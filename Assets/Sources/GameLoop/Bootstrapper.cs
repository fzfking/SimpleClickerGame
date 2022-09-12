using System;
using Sources.Data;
using Sources.GameLoop.States;
using Sources.Presenters.HelperViews;
using UnityEngine;

namespace Sources.GameLoop
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private ProgressBar ProgressBar;
        [SerializeField] private Canvas MainCanvas;
        private GameStateMachine _gameStateMachine;
        private void Awake()
        {
            _gameStateMachine = new GameStateMachine(StaticDataLoader.DataContainer, ProgressBar);
            _gameStateMachine.Enter<ResourcesInitState>();
        }
    }
}