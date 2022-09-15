using System;
using Sources.Architecture.Extensions;
using Sources.Architecture.Interfaces;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presenters
{
    public class ManagerPresenter : MonoBehaviour, IInitiable<IManager>, ILoadable
    {
        [SerializeField] private Button UnlockButton;
        [SerializeField] private TextMeshProUGUI UnlockButtonText;
        [SerializeField] private TextMeshProUGUI NameText;
        [SerializeField] private TextMeshProUGUI CostText;
        [SerializeField] private TextMeshProUGUI DescriptionText;
        [SerializeField] private Image ResourceIcon;
        [SerializeField] private Image GeneratorIcon;
        private IManager _manager;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        public void Init(IManager data)
        {
            _manager = data;
            InitViews();
        }

        private void InitViews()
        {
            NameText.text = _manager.Name;
            CostText.text = _manager.CostValue.ToResourceFormat();
            DescriptionText.text = _manager.Description;
            ResourceIcon.sprite = _manager.CostResource.Icon;
            GeneratorIcon.sprite = _manager.Generator.Icon;
            if (_manager.IsActive)
            {
                SetBuyedView();
            }
            else
            {
                SetLockedView();
            }
        }

        private void SetLockedView()
        {
            UnlockButton.interactable = true;
            UnlockButtonText.text = "Buy";
            UnlockButton.onClick.AsObservable().Subscribe(x => TryBuy()).AddTo(_compositeDisposable);
        }

        private void SetBuyedView()
        {
            UnlockButton.interactable = false;
            UnlockButtonText.text = "Already bought";
        }

        private void TryBuy()
        {
            if (_manager.CostResource.TrySpend(_manager.CostValue))
            {
                _manager.ChangeActive(true);
                SetBuyedView();
                _compositeDisposable.Clear();
            }
        }

        public void DeInit()
        {
            if (_compositeDisposable.Count > 0)
            {
                _compositeDisposable.Clear();
            }
        }
    }
}