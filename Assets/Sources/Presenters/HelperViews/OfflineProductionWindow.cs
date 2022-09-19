using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presenters.HelperViews
{
    public class OfflineProductionWindow : MonoBehaviour
    {
        [SerializeField] private ResourceProducedView ResourceProducedViewPrefab;
        [SerializeField] private Transform Container;
        [SerializeField] private Button CloseButton;
        [SerializeField] private TextMeshProUGUI TimePassedText;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private void Awake()
        {
            CloseButton.onClick.AsObservable().Subscribe(x => gameObject.SetActive(false)).AddTo(_compositeDisposable);
        }

        public void SetTimePassed(TimeSpan timeSpan)
        {
            TimePassedText.text = $"You were offline: {string.Format("d:{0:%d} h:{0:%h} m:{0:%m} s:{0:%s}", timeSpan)}";
        }

        public void Add(Sprite icon, string value)
        {
            var newProducedResourceView = Instantiate(ResourceProducedViewPrefab, Container);
            newProducedResourceView.Init(icon, value);
        }

        private void OnDisable()
        {
            _compositeDisposable.Clear();
        }
    }
}