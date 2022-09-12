using System;
using Sources.Data;
using Sources.GameLoop.States;
using Sources.Presenters;
using Sources.Presenters.HelperViews;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.GameLoop
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private ProgressBar ProgressBar;
        [SerializeField] private HorizontalLayoutGroup ResourcesParent;
        [SerializeField] private ResourcePresenter ResourcePresenterPrefab;
        private GameStateMachine _gameStateMachine;

        private void Awake()
        {
            _gameStateMachine = new GameStateMachine(StaticDataLoader.DataContainer, ProgressBar,
                ResourcePresenterPrefab, ResourcesParent.transform);
            _gameStateMachine.Enter<ResourcesInitState>();
        }
    }
}