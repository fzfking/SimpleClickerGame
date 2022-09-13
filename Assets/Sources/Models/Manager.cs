using Sources.Architecture.Interfaces;
using Sources.Data;
using UniRx;
using UnityEngine;

namespace Sources.Models
{
    public class Manager: IManager, IDeInitiable
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite Icon { get; }
        public IGenerator Generator => _generator;
        public bool IsActive => _isActive;

        private bool _isActive;
        private readonly IGenerator _generator;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        public Manager(IGenerator generator, ManagerData data)
        {
            _generator = generator;
            Name = data.Name;
            Description = data.Description;
            Icon = data.Icon;
        }

        private Manager(IGenerator generator)
        {
            _generator = generator;
        }

        public static IManager CreateMock(IGenerator generator)
        {
            return new Manager(generator);
        }

        public void ChangeActive(bool value)
        {
            _isActive = value;
            if (IsActive)
            {
                _generator.OnEnd.Subscribe(GeneratorOnEnded).AddTo(_compositeDisposable);
                _generator.TryProduce();
            }
            else
            {
                _compositeDisposable.Clear();
            }
        }

        private void GeneratorOnEnded(double value)
        {
            _generator.TryProduce();
        }

        public void DeInit()
        {
            _compositeDisposable.Clear();
        }
    }
}