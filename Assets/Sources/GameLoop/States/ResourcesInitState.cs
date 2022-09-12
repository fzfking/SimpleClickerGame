using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sources.Architecture.Interfaces;
using Sources.Data;
using Sources.Models;
using Sources.Presenters.HelperViews;
using UniRx;
using UnityEngine;

namespace Sources.GameLoop.States
{
    public class ResourcesInitState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ResourcesDataContainer _dataContainer;
        private readonly ProgressBar _progressBar;
        private readonly Dictionary<ResourceData, IResource> _resources;
        private readonly IGenerator[] _generators;

        public ResourcesInitState(GameStateMachine stateMachine, ResourcesDataContainer dataContainer, ProgressBar progressBar)
        {
            _stateMachine = stateMachine;
            _dataContainer = dataContainer;
            _progressBar = progressBar;
            _resources = new Dictionary<ResourceData, IResource>(_dataContainer.Resources.Length);
        }

        public void Enter()
        {
            
            MainThreadDispatcher.StartCoroutine(StartLoading());

        }

        private IEnumerator StartLoading()
        {
            _progressBar.gameObject.SetActive(true);
            yield return ResourcesInitialization();
            _stateMachine.Enter<ResourcesViewInitState, Dictionary<ResourceData, IResource>>(_resources); 
        }

        private IEnumerator ResourcesInitialization()
        {
            var resourcesData = _dataContainer.Resources;
            int i = 0;
            _progressBar.UpdateView(0f,
                $"Resources loading...");
            foreach (var resourceData in resourcesData)
            {
                _progressBar.UpdateView((i+0f) / resourcesData.Length,
                    $"{resourceData.Name} loading...");
                _resources.Add(resourceData, new Resource(0, resourceData));
                i++;
                yield return new WaitForSeconds(1f);
                _progressBar.UpdateView((i+0f) / resourcesData.Length,
                    $"{resourceData.Name} loaded.");
            }
            yield return new WaitForSeconds(0.5f);
        }

        public void Exit()
        {
        }
    }
}