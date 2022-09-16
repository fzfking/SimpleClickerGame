using System;
using Sources.Architecture.Interfaces;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presenters.HelperViews
{
    public class BuyAmountButton : MonoBehaviour, ILoadable
    {
        [SerializeField] private Button Button;
        [SerializeField] private TextMeshProUGUI AmountText;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private IBuyService _buyService;

        public void Init(IBuyService buyService)
        {
            _buyService = buyService;
            UpdateAmount();
            Button.onClick.AsObservable().Subscribe(x =>
            {
                _buyService.GoToNextAmount();
                UpdateAmount();
            }).AddTo(_compositeDisposable);
        }

        private void UpdateAmount()
        {
            AmountText.text = _buyService.GetCurrentAmount();
        }

        private void OnDestroy()
        {
            if (_compositeDisposable.Count > 0)
            {
                _compositeDisposable.Clear();
            }
        }
    }
}