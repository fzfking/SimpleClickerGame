using System;
using System.Linq;
using Configuration;
using UniRx;

namespace Domain
{
    public class Manager : IEquatable<Manager>
    {
        public byte Id => _def.Id;
        public IReadOnlyReactiveProperty<bool> IsUnlocked => _isUnlocked;
        
        public string NameKey => _def.NameKey;
        public string DescriptionKey => _def.DescriptionKey;
        public string IconKey => _def.IconKey;
        public string ImageKey => _def.ImageKey;
        public byte PriceResourceId => _def.Price.First().Key;
        public double Price => _def.Price.First().Value;

        private readonly ReactiveProperty<bool> _isUnlocked;
        private readonly ManagerDef _def;

        public Manager(ManagerDef def, bool isUnlocked)
        {
            _def = def;
            _isUnlocked = new(isUnlocked);
        }

        public void Unlock()
        {
            _isUnlocked.Value = true;
        }

        public bool Equals(Manager other)
        {
            return Id == other?.Id;
        }
    }
}