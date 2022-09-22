using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sources.Architecture.Extensions;
using Sources.Architecture.Interfaces;
using Sources.Data;
using Sources.Data.StaticViews;
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
        private readonly List<ISaveable> _saveables;
        private readonly Dictionary<ResourceData, IResource> _resources;
        private readonly Dictionary<GeneratorData, IGenerator> _generators;
        private readonly Dictionary<ManagerData, IManager> _managers;
        private ILoaderService _loaderService;

        private readonly Dictionary<IGenerator, GeneratorPresenter> _generatorPresenters =
            new Dictionary<IGenerator, GeneratorPresenter>();

        public InitState(GameStateMachine stateMachine, List<IDeInitiable> initiables, IServiceLocator allServices,
            List<ISaveable> saveables)
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
            var progressBar = _dataContainer.UIData.ProgressBar;
            progressBar.UpdateView(0f, "Loading models...");
            yield return InitModels();
            progressBar.UpdateView(0.25f, "Loading presenters...");
            yield return InitPresenters();
            progressBar.UpdateView(0.5f, "Loading buy mode button...");
            yield return InitBuyAmountButton();
            progressBar.UpdateView(0.75f, "Loading information windows...");
            yield return LinkInformationalsToService();
            progressBar.UpdateView(1f, "Almost done...");
            yield return null;
            progressBar.gameObject.SetActive(false);
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
            foreach (var resourceData in resourcesData)
            {

                IResource resource = Resource.Load(resourceData);
                _resources.Add(resourceData, resource);
                _saveables.Add(resource);
            }

            yield return null;
        }

        private IEnumerator InitGenerators()
        {
            var generatorsData = _dataContainer.GeneratorsDataContainer.Generators;
            foreach (var generatorData in generatorsData)
            {
                IGenerator generator = Generator.Load(generatorData,
                    _resources[generatorData.ProductionResource],
                    _resources[generatorData.CostResource]);
                _generators.Add(generatorData, generator);
                _saveables.Add(generator);
            }

            yield return null;
        }

        private IEnumerator InitManagers()
        {
            var managersData = _dataContainer.ManagersDataContainer.Managers;
            var offlineProductionWindow = _dataContainer.UIData.OfflineProductionWindow;
            var exitTime =
                DateTime.Parse(PlayerPrefs
                        .GetString("ExitTime", DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                    DateTimeFormatInfo.InvariantInfo);
            var timePassed = DateTime.Now - exitTime;
            bool isProducedAnyResourceOffline = false;
            offlineProductionWindow.gameObject.SetActive(true);
            offlineProductionWindow.SetTimePassed(timePassed);
            foreach (var managerData in managersData)
            {
                var manager = Manager.Load(managerData,
                    _generators[managerData.Generator],
                    _resources[managerData.Resource]);

                if (manager.IsActive && exitTime < DateTime.Now)
                {
                    CalculateOfflineProduction(timePassed, manager, offlineProductionWindow,
                        ref isProducedAnyResourceOffline);
                }

                _initiables.Add(manager);
                _managers.Add(managerData, manager);
                _saveables.Add(manager);
            }

            if (!isProducedAnyResourceOffline)
            {
                _dataContainer.UIData.OfflineProductionWindow.gameObject.SetActive(false);
            }

            yield return null;
        }

        private static void CalculateOfflineProduction(TimeSpan timePassed, Manager manager,
            OfflineProductionWindow offlineProductionWindow, ref bool isProducedAnyResourceOffline)
        {
            var producedCount = (int)(timePassed.TotalSeconds / manager.Generator.DelayTime);
            if (producedCount == 0)
            {
                return;
            }

            var producedValue = producedCount * manager.Generator.ProductionValue;
            offlineProductionWindow.Add(manager.Generator.ProductionResource.Icon,
                $"+{producedValue.ToResourceFormat()}");
            manager.Generator.ProductionResource.Increase(producedValue);
            isProducedAnyResourceOffline = true;
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