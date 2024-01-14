using Domain;
using Domain.Services;
using Domain.Services.Fulfillment;
using Zenject;

namespace Installers
{
    public class MainLoopInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<Repository<Resource>>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<Repository<Producer>>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<Repository<Manager>>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<ResourceFulfillmentService>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<ProducerFulfillmentService>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<ManagerFulfillmentService>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<Bootstrap>()
                .AsSingle();
            
            Container
                .BindInterfacesTo<ConfigLoader>()
                .AsSingle();
        }
    }
}