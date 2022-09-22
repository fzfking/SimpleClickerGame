using System;
using System.Collections.Generic;
using Sources.Architecture;
using Sources.Architecture.Interfaces;
using Sources.GameLoop.States;

namespace Sources.GameLoop
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;
        private readonly IServiceLocator _serviceLocator;
        private readonly List<IDeInitiable> _initiables;
        private readonly List<ISaveable> _saveables;

        public GameStateMachine()
        {
            _serviceLocator = new ServiceLocator();
            _initiables = new List<IDeInitiable>();
            _saveables = new List<ISaveable>();

            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(InitServicesState)] = new InitServicesState(this, _serviceLocator, _initiables),
                [typeof(InitState)] = new InitState(this, _initiables, _serviceLocator, _saveables),
                [typeof(GameLoopState)] = new GameLoopState(_serviceLocator, _initiables, _saveables),
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