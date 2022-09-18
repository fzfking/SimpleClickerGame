using Sources.Architecture.Extensions;
using Sources.Architecture.Interfaces;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presenters
{
    public class LockedGeneratorPresenter : MonoBehaviour, IInitiable<IGenerator>, ILoadable
    {
        [SerializeField] private TextMeshProUGUI NameText;
        [SerializeField] private TextMeshProUGUI CostText;
        [SerializeField] private Image ResourceIcon;
        [SerializeField] private Button UnlockButton;
        
        private IGenerator _generator;
        private GeneratorPresenter _presenter;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        public void Init(IGenerator data)
        {
            _generator = data;
            UnlockButton.onClick.AsObservable().Subscribe(x=> _generator.TryUpgrade(1)).AddTo(_compositeDisposable);
        }

        public void Link(GeneratorPresenter generatorPresenter)
        {
            _presenter = generatorPresenter;
            NameText.text = _generator.Name;
            CostText.text = _generator.CostValue.ToResourceFormat();
            ResourceIcon.sprite = _generator.CostResource.Icon;
            _generator.Level.Where(x => x > 0).Take(1).Subscribe(x =>
            {
                _presenter.gameObject.SetActive(true);
                DeInit();
            }).AddTo(_compositeDisposable);
        }

        public void DeInit()
        {
            gameObject.SetActive(false);
            if (_compositeDisposable.Count > 0)
            {
                _compositeDisposable.Clear();
            }

        }
    }
}