using System;
using System.Collections.Generic;
using Configuration;
using UniRx;

namespace Domain.Services.Fulfillment
{
    public class ProducerFulfillmentService : IDisposable
    {
        private readonly IRepository<Producer> _repository;
        private readonly CompositeDisposable _compositeDisposable = new();

        public ProducerFulfillmentService(IRepository<Producer> repository)
        {
            _repository = repository;
        }

        public void Fill(IEnumerable<ProducerDef> defs, PlayerProfile profile)
        {
            foreach (var def in defs)
            {
                var value = profile.Producers.GetValueOrDefault(def.Id, 0);
                var entity = new Producer(def, value);
                _repository.Add(entity);
                entity.Level
                    .Subscribe(x => profile.SetProducer(def.Id, x))
                    .AddTo(_compositeDisposable);
            }
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}