using System.Globalization;
using Sources.Architecture.Interfaces;
using UniRx;
using UnityEngine;

namespace Sources.Models
{
    public class Resource : IResource
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite Icon { get; }
        private readonly ReactiveProperty<double> _currentValue;
        public IReadOnlyReactiveProperty<double> CurrentValue => _currentValue;

        private Resource(double initialValue, ResourceData resourceData)
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

        public static Resource Load(ResourceData data)
        {
            var value = double.Parse(PlayerPrefs.GetString($"Resource: {data.Name}", "0"));
            return new Resource(value, data);
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
            if (_currentValue.Value - value >= 0)
            {
                _currentValue.Value -= value;
                return true;
            }

            return false;
        }

        public void Save()
        {
            PlayerPrefs.SetString($"Resource: {Name}", CurrentValue.Value.ToString(CultureInfo.InvariantCulture));
        }
    }
}