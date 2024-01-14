using System;
using System.Collections.Generic;
using Configuration;
using UniRx;

namespace Domain.Services.Fulfillment
{
    public class ResourceFulfillmentService : IDisposable
    {
        private readonly IRepository<Resource> _repository;
        private readonly CompositeDisposable _compositeDisposable = new();
        
        public ResourceFulfillmentService(IRepository<Resource> repository)
        {
            _repository = repository;
        }
        
        public void Fill(IEnumerable<ResourceDef> defs, PlayerProfile profile)
        {
            foreach (var def in defs)
            {
                var value = profile.Resources.GetValueOrDefault(def.Id, 0);
                var entity = new Resource(def, value);
                _repository.Add(entity);
                entity.Value
                    .Subscribe(x => profile.SetResource(def.Id, x))
                    .AddTo(_compositeDisposable);
            }
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}