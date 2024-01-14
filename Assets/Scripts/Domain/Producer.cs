using System;
using System.Linq;
using Configuration;
using UniRx;

namespace Domain
{
    public class Producer : IEquatable<Producer>
    {
        private static double ExponentialModifier => 1.05;
        
        public byte Id => _def.Id;
        public IReadOnlyReactiveProperty<double> Level => _level;
        public IReadOnlyReactiveProperty<double> CurrentProduction => _currentProduction;
        public IReadOnlyReactiveProperty<double> CurrentPrice => _currentPrice;
        public IReadOnlyReactiveProperty<TimeSpan> CurrentProductionTime => _currentProductionTime;
        
        public string NameKey => _def.NameKey;
        public string DescriptionKey => _def.DescriptionKey;
        public string IconKey => _def.IconKey;
        public string ImageKey => _def.ImageKey;
        public byte ProducedResourceId => _def.ProducedResourceId;
        
        public readonly byte PriceResourceId;

        private readonly ReactiveProperty<double> _level;
        private readonly ProducerDef _def;
        private readonly ReactiveProperty<double> _currentProduction;
        private readonly ReactiveProperty<double> _currentPrice;
        private readonly ReactiveProperty<TimeSpan> _currentProductionTime;
        private readonly double _basePrice;

        private double _levelsBuyAmount;

        public Producer(ProducerDef def, double value)
        {
            _def = def;
            _level = new(value);
            _currentProduction = new();
            _currentPrice = new();
            _currentProductionTime = new();
            PriceResourceId = _def.Price.First().Key;
            _basePrice = _def.Price.First().Value;
            _levelsBuyAmount = 1;
            UpdateProduction();
            UpdatePrice();
            UpdateProductionTime();
        }

        public void OffsetLevels(double levelsAmount)
        {
            _level.Value += levelsAmount;
            UpdateProduction();
            UpdatePrice();
            UpdateProductionTime();
        }

        public void ChangeLevelsBuyAmount(double amount)
        {
            _levelsBuyAmount = amount;
            UpdatePrice();
        }

        private void UpdateProduction()
        {
            var levelBonusMultiplier = _def.LevelMultiplier
                .Where(x => x.Key <= Level.Value)
                .Select(x => x.Value)
                .Aggregate((x,y) => x * y);
            _currentProduction.Value = _def.BaseProduction * Level.Value * levelBonusMultiplier;
        }

        private void UpdatePrice()
        {
            _currentPrice.Value = _basePrice * Math.Pow(ExponentialModifier, Level.Value + _levelsBuyAmount);
        }

        private void UpdateProductionTime()
        {
            var currentTimeInSeconds = _def.ProductionTimeByLevel
                .Where(x => x.Key <= Level.Value)
                .OrderByDescending(x => x.Key)
                .First().Value;
            _currentProductionTime.Value = TimeSpan.FromSeconds(currentTimeInSeconds);
        }

        public bool Equals(Producer other)
        {
            return Id == other?.Id;
        }
    }
}