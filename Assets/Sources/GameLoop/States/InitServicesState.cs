using System.Collections;
using Sources.Architecture.Interfaces;
using Sources.Data;
using Sources.GameLoop.Services;
using Sources.Presenters.HelperViews;
using UniRx;

namespace Sources.GameLoop.States
{
    public class InitServicesState: IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IServiceLocator _serviceLocator;

        public InitServicesState(GameStateMachine stateMachine, IServiceLocator serviceLocator)
        {
            _stateMachine = stateMachine;
            _serviceLocator = serviceLocator;
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
    }
}