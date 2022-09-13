using Sources.Architecture.Extensions;
using Sources.Architecture.Interfaces;
using Sources.Presenters;
using UniRx;
using UnityEngine;

namespace Sources.GameLoop.States
{
    public class GameLoopState: IPayloadableState<GeneratorPresenter[]>
    {
        private readonly IPopupService _popupService;
        private GeneratorPresenter[] _generatorPresenters;

        public GameLoopState(IPopupService popupService)
        {
            _popupService = popupService;
        }

        public void Enter(GeneratorPresenter[] payload)
        {
            _generatorPresenters = payload;
            foreach (var presenter in _generatorPresenters)
            {
                presenter.Ended += InvokePopup;
            }
        }

        public void Exit()
        {
            foreach (var presenter in _generatorPresenters)
            {
                presenter.Ended -= InvokePopup;
            }
        }

        private void InvokePopup(Vector2 position, double value)
        {
            _popupService.ShowPopup(position, $"+{value.ToResourceFormat()}");
        }
    }
}