using System.Collections;
using Sources.Architecture.Interfaces;
using Sources.Data;
using UniRx;
using UnityEngine;

namespace Sources.Models
{
    public class Generator: IGenerator, IProgressive
    {
        
        public string Name { get; }
        public string Description { get; }
        public Sprite Icon { get; }
        public IResource ProductionResource { get; }
        public IResource CostResource { get; }
        public int Level { get; private set; }
        public double ProductionValue => _baseProduction * Level;
        public double UpgradeCost => _baseUpgradeCost * Level;
        public float DelayTime => _baseDelayTime * 1f;
        public IReadOnlyReactiveProperty<float> Progress => _progress;

        private readonly double _baseProduction;
        private readonly double _baseUpgradeCost;
        private readonly float _baseDelayTime;
        private readonly ReactiveProperty<float> _progress;


        public Generator(GeneratorData data, IResource productionResource, IResource costResource, int level)
        {
            ProductionResource = productionResource;
            Level = level;
            Name = data.Name;
            Description = data.Description;
            Icon = data.Icon;
            _baseProduction = data.BaseProduction;
            _baseUpgradeCost = data.BaseCost;
            _baseDelayTime = data.BaseDelay;
            CostResource = costResource;
            _progress = new ReactiveProperty<float>(0);
        }

        private Generator(IResource resource, double baseCost, double baseProduction, float baseDelay)
        {
            ProductionResource = resource;
            CostResource = resource;
            _baseUpgradeCost = baseCost;
            _baseProduction = baseProduction;
            _baseDelayTime = baseDelay;
            Level = 1;
            _progress = new ReactiveProperty<float>(0);
        }

        public static Generator CreateMock(IResource resource, double baseCost = 0, double baseProduction = 0, float baseDelay = 0f)
        {
            return new Generator(resource, baseCost, baseProduction, baseDelay);
        }

        public void Produce()
        {
            MainThreadDispatcher.StartUpdateMicroCoroutine(WaitForProduce());
        }

        public bool TryUpgrade()
        {
            if (CostResource.TrySpend(UpgradeCost))
            {
                Level++;
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
            ProductionResource.Increase(ProductionValue);
        }
    }
}