using System;
using Sources.Architecture.Extensions;
using Sources.Architecture.Interfaces;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presenters
{
    public class ResourcePresenter : MonoBehaviour, IInitiable<IResource>, ILoadable, IInformational
    {
        public event Action<IVisualData> InfoNeeded;

        [SerializeField] private Image Icon;
        [SerializeField] private TextMeshProUGUI ValueText;
        [SerializeField] private Button InfoButton;
        private IResource _resource;
        private CompositeDisposable _compositeDisposable;

        public void Init(IResource resource)
        {
            _compositeDisposable = new CompositeDisposable();
            _resource = resource;
            Icon.sprite = _resource.Icon;
            _resource.CurrentValue.Subscribe(ChangeValueText).AddTo(_compositeDisposable);
            InfoButton.onClick.AsObservable().Subscribe(x => InfoNeeded?.Invoke(_resource))
                .AddTo(_compositeDisposable);
        }

        private void ChangeValueText(double value)
        {
            ValueText.text = value.ToResourceFormat();
        }

        public void DeInit()
        {
            _compositeDisposable.Clear();
            InfoNeeded = null;
        }
    }
}