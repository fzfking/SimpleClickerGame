using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sources.Architecture.Interfaces;
using Sources.Presenters;
using Sources.Presenters.HelperViews;
using UniRx;
using UnityEngine;

namespace Sources.GameLoop.States
{
    public class ResourcesViewInitState: IPayloadableState<Dictionary<ResourceData, IResource>>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ProgressBar _progressBar;
        private readonly ResourcePresenter _presenterPrefab;
        private readonly Transform _parent;
        private ResourcePresenter[] _resourcePresenters;

        public ResourcesViewInitState(GameStateMachine stateMachine, ProgressBar progressBar, ResourcePresenter presenterPrefab, Transform parent)
        {
            _stateMachine = stateMachine;
            _progressBar = progressBar;
            _presenterPrefab = presenterPrefab;
            _parent = parent;
        }
        public void Enter(Dictionary<ResourceData, IResource> payload)
        {
            MainThreadDispatcher.StartCoroutine(CreateViews(payload));
            _stateMachine.Enter<GeneratorsInitState, Dictionary<ResourceData, IResource>>(payload);
        }

        private IEnumerator CreateViews(Dictionary<ResourceData, IResource> payload)
        {
            _resourcePresenters = new ResourcePresenter[payload.Count];
            var resources = payload.Values.ToArray();
            _progressBar.UpdateView(0, "Resource views loading...");
            for (var i = 0; i < resources.Length; i++)
            {
                var resource = resources[i];
                _resourcePresenters[i] = GameObject.Instantiate<ResourcePresenter>(_presenterPrefab, _parent);
                _resourcePresenters[i].Init(resource);
                _progressBar.UpdateView((i+0f)/resources.Length, $"{resource.Name} view loading...");
                yield return null;
            }
        }

        public void Exit()
        {
            
        }
    }
}