using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sources.Architecture.Interfaces;
using Sources.Data;
using Sources.GameLoop.Services;
using Sources.Presenters;
using Sources.Presenters.HelperViews;
using UniRx;

namespace Sources.GameLoop.States
{
    public class InitServicesState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IServiceLocator _serviceLocator;
        private readonly List<IDeInitiable> _initiables;

        public InitServicesState(GameStateMachine stateMachine, IServiceLocator serviceLocator,
            List<IDeInitiable> initiables)
        {
            _stateMachine = stateMachine;
            _serviceLocator = serviceLocator;
            _initiables = initiables;
        }

        public void Exit()
        {
        }

        public void Enter()
        {
            MainThreadDispatcher.StartCoroutine(InitServices());
        }

        private IEnumerator InitServices()
        {
            var loaderService = InitLoaderService();
            yield return InitPopupService(loaderService);
            yield return InitBuyService();
            yield return InitInformationService(loaderService);
            _stateMachine.Enter<InitState>();
        }

        private ILoaderService InitLoaderService()
        {
            ILoaderService loaderService = new LoaderService();
            _serviceLocator.Add(loaderService);
            return loaderService;
        }

        private IEnumerator InitPopupService(ILoaderService loaderService)
        {
            var popup = loaderService.Load<Popup>();
            var popupParent = loaderService.Load<StaticDataContainer>().UIData.Get<IGenerator>().parent;
            IPopupService popupService = new PopupService(popup, popupParent);
            _serviceLocator.Add(popupService);
            yield return null;
        }

        private IEnumerator InitBuyService()
        {
            IBuyService buyService =
                new BuyService(_initiables.OfType<GeneratorPresenter>());
            _serviceLocator.Add(buyService);
            yield return null;
        }

        private IEnumerator InitInformationService(ILoaderService loaderService)
        {
            var window = loaderService.Load<StaticDataContainer>().UIData.InformationWindow;
            IInformationService informationService = new InformationService(window);
            _serviceLocator.Add(informationService);
            yield return null;
        }
    }
}