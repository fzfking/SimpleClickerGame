using System;
using System.Collections;
using Sources.Architecture.Interfaces;
using Sources.Data;
using UniRx;
using UnityEngine;

namespace Sources.Models
{
    public class Generator : IGenerator
    {
        public ReactiveCommand<double> OnEnd { get; private set; }
        public string Name { get; }
        public string Description { get; }
        public Sprite Icon { get; }
        public IResource ProductionResource { get; }
        public IResource CostResource { get; }

        public IReadOnlyReactiveProperty<int> Level => _level;

        public double ProductionValue => _baseProduction * Level.Value;
        public double CostValue => _baseUpgradeCost * (Level.Value + 1);
        public float DelayTime => _baseDelayTime * 1f;
        public IReadOnlyReactiveProperty<float> Progress => _progress;

        private readonly double _baseProduction;
        private readonly double _baseUpgradeCost;
        private readonly float _baseDelayTime;
        private readonly ReactiveProperty<float> _progress;
        private readonly ReactiveProperty<int> _level;


        public Generator(GeneratorData data, IResource productionResource, IResource costResource, int level = 0)
        {
            ProductionResource = productionResource;
            _level = new ReactiveProperty<int>(level);
            Name = data.Name;
            Description = data.Description;
            Icon = data.Icon;
            _baseProduction = data.BaseProduction;
            _baseUpgradeCost = data.BaseCost;
            _baseDelayTime = data.BaseDelay;
            CostResource = costResource;
            _progress = new ReactiveProperty<float>(0);
            OnEnd = new ReactiveCommand<double>(Progress.Select(p => p == 0f).AsObservable(), true);
        }

        private Generator(IResource resource, double baseCost, double baseProduction, float baseDelay)
        {
            ProductionResource = resource;
            CostResource = resource;
            _baseUpgradeCost = baseCost;
            _baseProduction = baseProduction;
            _baseDelayTime = baseDelay;
            _level = new ReactiveProperty<int>(1);
            _progress = new ReactiveProperty<float>(0);
            OnEnd = new ReactiveCommand<double>(Progress.Select(p => p == 0f).AsObservable(), true);
        }

        public static Generator CreateMock(IResource resource, double baseCost = 0, double baseProduction = 0,
            float baseDelay = 0f)
        {
            return new Generator(resource, baseCost, baseProduction, baseDelay);
        }

        public void TryProduce()
        {
            if (Progress.Value == 0f)
            {
                Produce();
            }
        }

        private void Produce()
        {
            MainThreadDispatcher.StartUpdateMicroCoroutine(WaitForProduce());
        }

        public bool CanUpgrade(int levelValue)
        {
            return (CostResource.CurrentValue.Value - CostValue) >= 0f;
        }

        public bool TryUpgrade()
        {
            if (CostResource.TrySpend(CostValue))
            {
                _level.Value++;
                return true;
            }

            return false;
        }

        private IEnumerator WaitForProduce()
        {
            var timePassed = 0f;
            while (timePassed < DelayTime)
            {
                timePassed += Time.deltaTime;
                _progress.Value = timePassed / DelayTime;
                yield return null;
            }

            _progress.Value = 0f;
            InvokeEnded();
            ProductionResource.Increase(ProductionValue);
        }

        private void InvokeEnded()
        {
            OnEnd?.Execute(ProductionValue);
        }
    }
}