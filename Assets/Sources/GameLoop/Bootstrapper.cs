using System;
using Sources.Architecture.Interfaces;
using Sources.Data;
using Sources.Data.StaticViews;
using Sources.GameLoop.Services;
using Sources.GameLoop.States;
using Sources.Presenters;
using Sources.Presenters.HelperViews;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.GameLoop
{
    public class Bootstrapper : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine;

        private void Awake()
        {
            _gameStateMachine = new GameStateMachine();
            Input.backButtonLeavesApp = true;
            _gameStateMachine.Enter<InitServicesState>();
        }

        private void OnApplicationQuit()
        {
            _gameStateMachine.Exit();
        }
    }
}