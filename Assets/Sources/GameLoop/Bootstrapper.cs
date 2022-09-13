using System;
using Sources.Architecture.Interfaces;
using Sources.Data;
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
        [SerializeField] private ProgressBar ProgressBar;
        [SerializeField] private HorizontalLayoutGroup ResourcesParent;
        [SerializeField] private ResourcePresenter ResourcePresenterPrefab;
        [SerializeField] private VerticalLayoutGroup GeneratorsParent;
        [SerializeField] private GeneratorPresenter GeneratorPresenterPrefab;
        private GameStateMachine _gameStateMachine;
        private ILoaderService _loaderService;

        private void Awake()
        {
            _loaderService = new LoaderService();
            _gameStateMachine = new GameStateMachine(_loaderService, ProgressBar, ResourcesParent.transform,
                GeneratorsParent.transform);
            _gameStateMachine.Enter<ResourcesInitState>();
        }

        private void OnDestroy()
        {
            _gameStateMachine.Exit();
        }
    }
}