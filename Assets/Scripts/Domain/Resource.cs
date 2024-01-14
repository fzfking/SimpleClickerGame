using System;
using Configuration;
using UniRx;

namespace Domain
{
    public class Resource : IEquatable<Resource>
    {
        public byte Id => _def.Id;
        public IReadOnlyReactiveProperty<double> Value => _value;
        
        public string NameKey => _def.NameKey;
        public string DescriptionKey => _def.DescriptionKey;
        public string IconKey => _def.IconKey;
        public string ImageKey => _def.ImageKey;

        private readonly ReactiveProperty<double> _value;
        private readonly ResourceDef _def;

        public Resource(ResourceDef def, double value)
        {
            _def = def;
            _value = new(value);
        }

        public void Offset(double offset)
        {
            _value.Value += offset;
        }

        public bool Equals(Resource other)
        {
            return Id == other?.Id;
        }
    }
}