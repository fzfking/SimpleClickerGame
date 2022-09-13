using System.Collections;
using Sources.Architecture.Interfaces;
using Sources.Presenters;
using Sources.Presenters.HelperViews;
using UniRx;
using UnityEngine;

namespace Sources.GameLoop.States
{
    public class GeneratorViewsInitState: IPayloadableState<IGenerator[]>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ProgressBar _progressBar;
        private readonly GeneratorPresenter _presenterPrefab;
        private readonly Transform _parent;
        private GeneratorPresenter[] _generatorPresenters;

        public GeneratorViewsInitState(GameStateMachine stateMachine, ProgressBar progressBar, GeneratorPresenter presenterPrefab, Transform parent)
        {
            _stateMachine = stateMachine;
            _progressBar = progressBar;
            _presenterPrefab = presenterPrefab;
            _parent = parent;
        }
        public void Enter(IGenerator[] payload)
        {
            MainThreadDispatcher.StartCoroutine(CreateViews(payload));
        }

        private IEnumerator CreateViews(IGenerator[] payload)
        {
            _generatorPresenters = new GeneratorPresenter[payload.Length];
            _progressBar.UpdateView(0f, "Generator views loading...");
            for (int i = 0; i < payload.Length; i++)
            {
                _progressBar.UpdateView(0f, $"{payload[i].Name} view loading...");
                _generatorPresenters[i] = GameObject.Instantiate(_presenterPrefab, _parent);
                _generatorPresenters[i].Init(payload[i]);
                _progressBar.UpdateView(0f, $"{payload[i].Name} view loaded...");
                yield return null;
            }
            _progressBar.gameObject.SetActive(false);
            _stateMachine.Enter<GameLoopState, GeneratorPresenter[]>(_generatorPresenters);
        }

        public void Exit()
        {
            
        }
    }
}