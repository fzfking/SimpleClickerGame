using System;
using System.Globalization;
using DG.Tweening;
using Sources.Architecture.Extensions;
using Sources.Architecture.Interfaces;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presenters
{
    public class GeneratorPresenter : MonoBehaviour, IInitiable<IGenerator>, ILoadable, IInformational
    {
        public event Action<Vector2, double> Ended;
        public event Action<IVisualData> InfoNeeded;

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
        private RectTransform _rectTransform;
        private int _levelsAmount = 1;


        public void Init(IGenerator data)
        {
            _generator = data;
            CostResourceIcon.sprite = _generator.CostResource.Icon;
            ProductionResourceIcon.sprite = _generator.ProductionResource.Icon;
            GeneratorIcon.sprite = _generator.Icon;

            _generator.Level.Subscribe(UpdateView).AddTo(_compositeDisposable);

            _generator.Progress.Subscribe(UpdateProgress).AddTo(_compositeDisposable);

            ProductionButton.onClick.AsObservable()
                .Subscribe(x => Produce()).AddTo(_compositeDisposable);

            UpgradeButton.onClick.AsObservable().Where(x => _generator.CanUpgrade(1))
                .Subscribe(x => Upgrade()).AddTo(_compositeDisposable);

            _rectTransform = ProductionButton.GetComponent<RectTransform>();
            _generator.OnEnd.Subscribe(GeneratorOnEnded).AddTo(_compositeDisposable);
            _generator.CostResource.CurrentValue.Subscribe(x => ChangeLevelsAmount(_levelsAmount))
                .AddTo(_compositeDisposable);

            InfoButton.onClick.AsObservable().Subscribe(x => InfoNeeded?.Invoke(_generator))
                .AddTo(_compositeDisposable);
        }

        public void ChangeLevelsAmount(int levels)
        {
            _levelsAmount = levels;
            BuyAmount.text = levels == -1 ? "Max" : $"+{levels.ToString()}";
            UpdateLevelsCost();
        }

        private void GeneratorOnEnded(double value)
        {
            Ended?.Invoke(_rectTransform.position, value);
        }

        private void Upgrade()
        {
            _generator.TryUpgrade(_levelsAmount);
        }

        private void Produce()
        {
            _generator.TryProduce();
        }

        private void UpdateProgress(float value)
        {
            ProgressSlider.DOValue(value, 0.01f);
            Delay.text = ((1 - value) * _generator.DelayTime).ToString("##0.##", CultureInfo.InvariantCulture) + 'S';
        }

        private void UpdateView(int level)
        {
            Level.text = $"{level}";
            UpdateLevelsCost();
            Production.text = '+' + _generator.ProductionValue.ToResourceFormat();
            Delay.text = _generator.DelayTime.ToString(CultureInfo.InvariantCulture) + 'S';
        }

        private void UpdateLevelsCost()
        {
            Cost.text = '-' + _generator.GetCost(_levelsAmount).ToResourceFormat();
        }

        public void DeInit()
        {
            _compositeDisposable.Clear();
            Ended = null;
            InfoNeeded = null;
        }
    }
}