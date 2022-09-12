using System.Globalization;
using DG.Tweening;
using Sources.Architecture.Extensions;
using Sources.Architecture.Interfaces;
using Sources.Models;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presenters
{
    public class GeneratorPresenter : MonoBehaviour, IInitiable<IGenerator>
    {
        [SerializeField] private Image CostResourceIcon;
        [SerializeField] private Image ProductionResourceIcon;
        [SerializeField] private Image GeneratorIcon;
        [SerializeField] private Button ProductionButton;
        [SerializeField] private Button UpgradeButton;
        [SerializeField] private Button InfoButton;
        [SerializeField] private Slider ProgressSlider;
        [SerializeField] private TextMeshProUGUI Cost;
        [SerializeField] private TextMeshProUGUI Production;
        [SerializeField] private TextMeshProUGUI BuyAmount;
        [SerializeField] private TextMeshProUGUI Delay;
        [SerializeField] private TextMeshProUGUI Level;

        private IGenerator _generator;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();


        public void Init(IGenerator data)
        {
            _generator = data;
            CostResourceIcon.sprite = _generator.CostResource.Icon;
            ProductionResourceIcon.sprite = _generator.ProductionResource.Icon;
            GeneratorIcon.sprite = _generator.Icon;
            
            _generator.Level.Subscribe(UpdateView).AddTo(_compositeDisposable);
            
            _generator.Progress.Subscribe(UpdateProgress).AddTo(_compositeDisposable);

            ProductionButton.onClick.AsObservable().Where(x => _generator.Progress.Value == 0f)
                .Subscribe(x => Produce()).AddTo(_compositeDisposable);
            
            UpgradeButton.onClick.AsObservable().Where(x => _generator.CanUpgrade(1))
                .Subscribe(x => Upgrade()).AddTo(_compositeDisposable);
        }

        private void Upgrade()
        {
            _generator.TryUpgrade();
        }

        private void Produce()
        {
            _generator.Produce();
        }

        private void UpdateProgress(float value)
        {
            ProgressSlider.DOValue(value, 0.01f);
            Delay.text = ((1-value) * _generator.DelayTime).ToString("##0.##", CultureInfo.InvariantCulture) + 'S';
        }

        private void UpdateView(int level)
        {
            Level.text = $"{level}";
            Cost.text = '-' +_generator.UpgradeCost.ToResourceFormat();
            Production.text = '+' + _generator.ProductionValue.ToResourceFormat();
            Delay.text = _generator.DelayTime.ToString(CultureInfo.InvariantCulture) + 'S';
        }

        public void DeInit()
        {
            _compositeDisposable.Clear();
        }
    }
}