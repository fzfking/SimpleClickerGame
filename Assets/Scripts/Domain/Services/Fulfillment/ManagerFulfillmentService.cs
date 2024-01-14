using System;
using System.Collections.Generic;
using Configuration;
using UniRx;

namespace Domain.Services.Fulfillment
{
    public class ManagerFulfillmentService : IDisposable
    {
        private readonly IRepository<Manager> _repository;
        private readonly CompositeDisposable _compositeDisposable = new();

        public ManagerFulfillmentService(IRepository<Manager> repository)
        {
            _repository = repository;
        }

        public void Fill(IEnumerable<ManagerDef> defs, PlayerProfile profile)
        {
            foreach (var def in defs)
            {
                var value = profile.Managers.GetValueOrDefault(def.Id, false);
                var entity = new Manager(def, value);
                _repository.Add(entity);
                
                entity.IsUnlocked
                    .Subscribe(x => profile.SetManager(def.Id, x))
                    .AddTo(_compositeDisposable);
            }
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}