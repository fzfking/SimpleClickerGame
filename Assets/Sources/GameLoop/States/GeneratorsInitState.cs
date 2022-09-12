using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sources.Architecture.Interfaces;
using Sources.Data;
using Sources.Models;
using Sources.Presenters.HelperViews;
using UniRx;
using UnityEngine;

namespace Sources.GameLoop.States
{
    public class GeneratorsInitState : IPayloadableState<Dictionary<ResourceData, IResource>>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly GeneratorsDataContainer _dataContainer;
        private readonly ProgressBar _progressBar;
        private Dictionary<ResourceData, IResource> _resources;
        private Dictionary<GeneratorData, IGenerator> _generators;

        public GeneratorsInitState(GameStateMachine stateMachine, GeneratorsDataContainer dataContainer,
            ProgressBar progressBar)
        {
            _stateMachine = stateMachine;
            _dataContainer = dataContainer;
            _progressBar = progressBar;
            _generators = new Dictionary<GeneratorData, IGenerator>(dataContainer.Generators.Length);
        }

        public void Enter(Dictionary<ResourceData, IResource> payload)
        {
            _resources = payload;
            MainThreadDispatcher.StartCoroutine(GeneratorsInitialization());
        }

        private IEnumerator GeneratorsInitialization()
        {
            var generatorsData = _dataContainer.Generators;
            int i = 0;
            _progressBar.UpdateView(0f,
                $"Resources loading...");
            foreach (var generatorData in generatorsData)
            {
                _progressBar.UpdateView((i + 0f) / generatorsData.Length,
                    $"{generatorData.Name} loading...");
                _generators.Add(generatorData, new Generator(generatorData,
                    _resources[generatorData.ProductionResource],
                    _resources[generatorData.CostResource], 1));
                i++;
                yield return new WaitForSeconds(1f);
                _progressBar.UpdateView((i + 0f) / generatorsData.Length,
                    $"{generatorData.Name} loaded.");
            }
            yield return new WaitForSeconds(0.5f);
            _stateMachine.Enter<GeneratorViewsInitState, IGenerator[]>(_generators.Values.ToArray());
        }

        public void Exit()
        {
        }
    }
}