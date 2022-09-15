using System.Globalization;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Data.StaticViews
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Slider Slider;
        [SerializeField] private TextMeshProUGUI PercentText;
        [SerializeField] private TextMeshProUGUI ProgressDescription;
        private ReactiveProperty<float> _progressValue;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private void OnEnable()
        {
            _progressValue = new ReactiveProperty<float>(0f);
            _progressValue.Subscribe(UpdateValues).AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable.Clear();
            _progressValue = null;
        }

        public void UpdateView(float progress, string description = "")
        {
            _progressValue.Value = progress;
            ProgressDescription.text = description;
        }

        private void UpdateValues(float newValue)
        {
            Slider.DOValue(newValue, 0.05f, false);
            PercentText.text = newValue.ToString("###0.##%", CultureInfo.InvariantCulture);
        }
    }
}