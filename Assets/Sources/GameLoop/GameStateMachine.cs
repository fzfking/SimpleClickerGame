using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sources.Architecture.Interfaces;
using Sources.Data;
using Sources.GameLoop.States;
using Sources.Presenters;
using Sources.Presenters.HelperViews;
using UniRx;
using UnityEngine;

namespace Sources.GameLoop
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;

        public GameStateMachine(StaticDataContainer dataContainer, ProgressBar progressBar,
            ResourcePresenter resourcePresenter, Transform resourcesParent, GeneratorPresenter generatorPresenter,
            Transform generatorsParent)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(ResourcesInitState)] =
                    new ResourcesInitState(this, dataContainer.ResourcesDataContainer, progressBar),
                [typeof(GeneratorsInitState)] =
                    new GeneratorsInitState(this, dataContainer.GeneratorsDataContainer, progressBar),
                [typeof(ResourcesViewInitState)] =
                    new ResourcesViewInitState(this, progressBar, resourcePresenter, resourcesParent),
                [typeof(GeneratorViewsInitState)] =
                    new GeneratorViewsInitState(this, progressBar, generatorPresenter, generatorsParent),
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

        private TState GetState<TState>() where TState : class, IExitableState => _states[typeof(TState)] as TState;
    }
}