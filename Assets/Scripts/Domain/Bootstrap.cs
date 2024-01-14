using System;
using System.Collections.Generic;
using Configuration;
using Cysharp.Threading.Tasks;
using Domain.Services;
using Domain.Services.Fulfillment;
using Zenject;

namespace Domain
{
    public class Bootstrap : IInitializable, IDisposable
    {
        private readonly IConfigLoader _configLoader;
        private readonly ResourceFulfillmentService _resourceFulfillmentService;
        private readonly ProducerFulfillmentService _producerFulfillmentService;
        private readonly ManagerFulfillmentService _managerFulfillmentService;
        private PlayerProfile _playerProfile;

        public Bootstrap(IConfigLoader configLoader, 
            ResourceFulfillmentService resourceFulfillmentService, 
            ProducerFulfillmentService producerFulfillmentService, 
            ManagerFulfillmentService managerFulfillmentService)
        {
            _configLoader = configLoader;
            _resourceFulfillmentService = resourceFulfillmentService;
            _producerFulfillmentService = producerFulfillmentService;
            _managerFulfillmentService = managerFulfillmentService;
        }
        
        public void Initialize()
        {
            InitializeAsync().Forget();

            async UniTask InitializeAsync()
            {
                var resourcesDefs = await _configLoader.LoadAsync<List<ResourceDef>>("resources");
                var producersDefs = await _configLoader.LoadAsync<List<ProducerDef>>("producers");
                var managersDefs = await _configLoader.LoadAsync<List<ManagerDef>>("managers");
                var gameDefaults = await _configLoader.LoadAsync<NewGameDef>("newGame");
                _playerProfile = PlayerProfile.Load(gameDefaults);
                _resourceFulfillmentService.Fill(resourcesDefs, _playerProfile);
                _producerFulfillmentService.Fill(producersDefs, _playerProfile);
                _managerFulfillmentService.Fill(managersDefs, _playerProfile);
            }
        }

        public void Dispose()
        {
            _playerProfile?.Dispose();
        }
    }
}