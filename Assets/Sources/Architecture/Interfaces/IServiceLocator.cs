namespace Sources.Architecture.Interfaces
{
    public interface IServiceLocator
    {
        TService Add<TService>(TService service) where TService : IService;
        TService Get<TService>() where TService : IService;
        void Remove<TService>() where TService : IService;
    }
}