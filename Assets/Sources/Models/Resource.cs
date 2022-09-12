using Sources.Architecture.Interfaces;
using UniRx;
using UnityEngine;

namespace Sources.Models
{
    public class Resource : IResource, IVisualData
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite Icon { get; }
        private readonly ReactiveProperty<double> _currentValue;
        public IReadOnlyReactiveProperty<double> CurrentValue => _currentValue;

        public Resource(double initialValue, ResourceData resourceData)
        {
            Name = resourceData.Name;
            Description = resourceData.Description;
            Icon = resourceData.Icon;
            _currentValue = new ReactiveProperty<double>(initialValue);
        }

        private Resource(double value = 0)
        {
            _currentValue = new ReactiveProperty<double>(value);
        }

        public static Resource CreateMock(double initialValue)
        {
            return new Resource();
        }

        public void Increase(double value)
        {
            _currentValue.Value += value;
        }

        public bool TrySpend(double value)
        {
            if (_currentValue.Value - value > 0)
            {
                _currentValue.Value -= value;
                return true;
            }

            return false;
        }
    }
}