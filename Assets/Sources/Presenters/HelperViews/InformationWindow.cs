using Sources.Architecture.Interfaces;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presenters.HelperViews
{
    public class InformationWindow : MonoBehaviour
    {
        [SerializeField] private Image Icon;
        [SerializeField] private TextMeshProUGUI Name;
        [SerializeField] private TextMeshProUGUI Description;
        [SerializeField] private Button CloseButton;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private void Awake()
        {
            CloseButton.onClick.AsObservable().Subscribe(x => gameObject.SetActive(false))
                .AddTo(_compositeDisposable);
        }

        public void Show(IVisualData data)
        {
            Icon.sprite = data.Icon;
            Name.text = data.Name;
            Description.text = data.Description;
        }

        private void OnDestroy()
        {
            _compositeDisposable.Clear();
        }
    }
}