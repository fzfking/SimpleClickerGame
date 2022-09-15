using System.Collections.Generic;
using Sources.Architecture.Extensions;
using Sources.Architecture.Interfaces;
using Sources.Presenters;
using UniRx;
using UnityEngine;

namespace Sources.GameLoop.States
{
    public class GameLoopState: IPayloadableState<GeneratorPresenter[]>
    {
        private readonly IServiceLocator _allServices;
        private readonly List<IDeInitiable> _initiables;
        private IPopupService _popupService;
        private GeneratorPresenter[] _generatorPresenters;

        public GameLoopState(IServiceLocator allServices, List<IDeInitiable> initiables)
        {
            _allServices = allServices;
            _initiables = initiables;
        }

        public void Enter(GeneratorPresenter[] payload)
        {
            _popupService = _allServices.Get<IPopupService>();
            _generatorPresenters = payload;
            foreach (var presenter in _generatorPresenters)
            {
                presenter.Ended += InvokePopup;
            }
        }

        public void Exit()
        {
            foreach (var initiable in _initiables)
            {
                initiable.DeInit();
            }
        }

        private void InvokePopup(Vector2 position, double value)
        {
            _popupService.ShowPopup(position, $"+{value.ToResourceFormat()}");
        }
    }
}