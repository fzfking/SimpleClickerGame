using System;
using System.Collections.Generic;
using Sources.Architecture.Interfaces;
using Sources.Data;
using Sources.GameLoop.Services;
using Sources.GameLoop.States;
using Sources.Presenters;
using Sources.Presenters.HelperViews;
using UnityEngine;

namespace Sources.GameLoop
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;

        public GameStateMachine(ILoaderService loaderService, ProgressBar progressBar, Transform resourcesParent,
            Transform generatorsParent)
        {
            var staticDataContainer = loaderService.Load<StaticDataContainer>();
            var resourcesDataContainer = staticDataContainer.ResourcesDataContainer;
            var generatorsDataContainer = staticDataContainer.GeneratorsDataContainer;
            var resourcePresenter = loaderService.Load<ResourcePresenter>();
            var generatorPresenter = loaderService.Load<GeneratorPresenter>();
            var popupService = new PopupService(loaderService.Load<Popup>(), generatorsParent.root);
            
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(ResourcesInitState)] =
                    new ResourcesInitState(this, resourcesDataContainer, progressBar),
                [typeof(GeneratorsInitState)] =
                    new GeneratorsInitState(this, generatorsDataContainer, progressBar),
                [typeof(ResourcesViewInitState)] =
                    new ResourcesViewInitState(this, progressBar, resourcePresenter, resourcesParent),
                [typeof(GeneratorViewsInitState)] =
                    new GeneratorViewsInitState(this, progressBar, generatorPresenter, generatorsParent),
                [typeof(GameLoopState)] = new GameLoopState(popupService),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            _currentState?.Exit();
            var newState = GetState<TState>();
            newState.Enter();
            _currentState = newState;
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadableState<TPayload>
        {
            _currentState?.Exit();
            var newState = GetState<TState>();
            newState.Enter(payload);
            _currentState = newState;
        }

        public void Exit()
        {
            _currentState?.Exit();
        }

        private TState GetState<TState>() where TState : class, IExitableState => _states[typeof(TState)] as TState;
    }
}