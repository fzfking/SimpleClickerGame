using System;
using System.Collections.Generic;
using Sources.Architecture.Interfaces;

namespace Sources.Architecture
{
    public class ServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        public TService Add<TService>(TService service) where TService : IService
        {
            if (_services.TryAdd(typeof(TService), service))
            {
                return service;
            }

            throw new Exception($"You trying add service that are already exists in dictionary: {typeof(TService)}");
        }

        public TService Get<TService>() where TService : IService
        {
            if (_services.TryGetValue(typeof(TService), out var value))
            {
                return (TService)value;
            }

            throw new Exception($"You trying get service that not exists in dictionary: {typeof(TService)}");
        }

        public void Remove<TService>() where TService : IService
        {
            if (_services.Remove(typeof(TService)))
            {
                return;
            }

            throw new Exception($"You trying remove service that not exists in dictionary: {typeof(TService)}");
        }
    }
}