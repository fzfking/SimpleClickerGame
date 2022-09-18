using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sources.Architecture.Interfaces;
using Sources.Data;
using Sources.Data.ProgressContainers;
using Sources.Data.StaticViews;
using Sources.GameLoop.Services;
using Sources.Models;
using Sources.Presenters;
using Sources.Presenters.HelperViews;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.GameLoop.States
{
    public class InitState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private StaticDataContainer _dataContainer;
        private ProgressBar _progressBar;
        private readonly List<IDeInitiable> _initiables;
        private readonly IServiceLocator _allServices;
        private readonly List<IVisualData> _saveables;
        private readonly Dictionary<ResourceData, IResource> _resources;
        private readonly Dictionary<GeneratorData, IGenerator> _generators;
        private readonly Dictionary<ManagerData, IManager> _managers;
        private ILoaderService _loaderService;

        private readonly Dictionary<IGenerator, GeneratorPresenter> _generatorPresenters =
            new Dictionary<IGenerator, GeneratorPresenter>();

        public InitState(GameStateMachine stateMachine, List<IDeInitiable> initiables, IServiceLocator allServices,
            List<IVisualData> saveables)
        {
            _stateMachine = stateMachine;
            _initiables = initiables;
            _allServices = allServices;
            _saveables = saveables;
            _resources = new Dictionary<ResourceData, IResource>();
            _generators = new Dictionary<GeneratorData, IGenerator>();
            _managers = new Dictionary<ManagerData, IManager>();
        }

        public void Exit()
        {
        }

        public void Enter()
        {
            _dataContainer = _allServices.Get<ILoaderService>().Load<StaticDataContainer>();
            _progressBar = _dataContainer.UIData.ProgressBar;
            MainThreadDispatcher.StartCoroutine(InitAll());
        }

        private IEnumerator InitAll()
        {
            yield return InitModels();
            yield return InitPresenters();
            yield return InitBuyAmountButton();
            yield return LinkInformationalsToService();
            _stateMachine.Enter<GameLoopState, GeneratorPresenter[]>(_initiables.OfType<GeneratorPresenter>()
                .ToArray());
        }

        private IEnumerator InitModels()
        {
            yield return InitResources();
            yield return InitGenerators();
            yield return InitManagers();
        }

        private IEnumerator InitResources()
        {
            var resourcesData = _dataContainer.ResourcesDataContainer.Resources;
            var progressLoader = _allServices.Get<IProgressLoaderService>();
            foreach (var resourceData in resourcesData)
            {
                ResourceProgressContainer loadedData =
                    progressLoader.Load<IResource>(resourceData.Name) as ResourceProgressContainer;
                if (loadedData == null)
                {
                    throw new Exception($"Cannot load resource data {resourceData.Name}");
                }

                IResource resource = new Resource(loadedData.Value, resourceData);
                _resources.Add(resourceData, resource);
                _saveables.Add(resource);
            }

            yield return null;
        }

        private IEnumerator InitGenerators()
        {
            var generatorsData = _dataContainer.GeneratorsDataContainer.Generators;
            var progressLoader = _allServices.Get<IProgressLoaderService>();
            foreach (var generatorData in generatorsData)
            {
                GeneratorProgressContainer loadedData =
                    progressLoader.Load<IGenerator>(generatorData.Name) as GeneratorProgressContainer;
                if (loadedData == null)
                {
                    throw new Exception($"Cannot load generator data {generatorData.Name}");
                }

                var level = loadedData.Level > 0 ? loadedData.Level : generatorData.IsUnlockedByDefault ? 1 : 0;
                IGenerator generator = new Generator(generatorData,
                    _resources[generatorData.ProductionResource],
                    _resources[generatorData.CostResource], level, loadedData.Progress);
                _generators.Add(generatorData, generator);
                _saveables.Add(generator);
            }

            yield return null;
        }

        private IEnumerator InitManagers()
        {
            var managersData = _dataContainer.ManagersDataContainer.Managers;
            var progressLoader = _allServices.Get<IProgressLoaderService>();
            foreach (var managerData in managersData)
            {
                ManagerProgressContainer loadedData =
                    progressLoader.Load<IManager>(managerData.Name) as ManagerProgressContainer;
                if (loadedData == null)
                {
                    throw new Exception($"Cannot load generator data {managerData.Name}");
                }

                var manager = new Manager(_generators[managerData.Generator],
                    _resources[managerData.Resource],
                    managerData, loadedData.IsBuyed);
                _initiables.Add(manager);
                _managers.Add(managerData, manager);
                _saveables.Add(manager);
            }

            yield return null;
        }

        private IEnumerator InitPresenters()
        {
            yield return InitPresenters<ResourcePresenter, IResource>(_resources.Values.ToArray());
            yield return InitPresenters<GeneratorPresenter, IGenerator>(_generators.Values.ToArray());
            yield return InitPresenters<ManagerPresenter, IManager>(_managers.Values.ToArray());
            yield return InitLockedGeneratorPresenters();
        }

        private IEnumerator InitPresenters<TPresenter, TData>(TData[] values)
            where TPresenter : Object, ILoadable, IInitiable<TData>
            where TData : IVisualData
        {
            var prefab = _allServices.Get<ILoaderService>().Load<TPresenter>();
            var isGenerator = prefab.GetType() == typeof(GeneratorPresenter);
            foreach (var value in values)
            {
                var presenter = Object.Instantiate(prefab, _dataContainer.UIData.Get<TData>());
                presenter.Init(value);
                _initiables.Add(presenter);
                if (isGenerator)
                {
                    _generatorPresenters.Add((IGenerator)value, presenter as GeneratorPresenter);
                }
            }

            yield return null;
        }

        private IEnumerator InitLockedGeneratorPresenters()
        {
            var prefab = _allServices.Get<ILoaderService>().Load<LockedGeneratorPresenter>();
            var parent = _dataContainer.UIData.Get<IGenerator>();
            foreach (var pair in _generatorPresenters.Where(x => x.Key.Level.Value == 0))
            {
                pair.Value.gameObject.SetActive(false);
                var locked = GameObject.Instantiate(prefab, parent);
                locked.Init(pair.Key);
                locked.Link(pair.Value);
                _initiables.Add(locked);
            }

            yield return null;
        }

        private IEnumerator InitBuyAmountButton()
        {
            var button = _dataContainer.UIData.BuyAmountButton;
            var buyService = _allServices.Get<IBuyService>();
            button.Init(buyService);
            buyService.Enable();
            yield return null;
        }

        private IEnumerator LinkInformationalsToService()
        {
            var informationals = _initiables.OfType<IInformational>();

            void InvokeInformationView(IVisualData data)
            {
                _allServices.Get<IInformationService>().ShowInfo(data);
            }

            foreach (var informational in informationals)
            {
                informational.InfoNeeded += InvokeInformationView;
            }

            yield return null;
        }
    }
}