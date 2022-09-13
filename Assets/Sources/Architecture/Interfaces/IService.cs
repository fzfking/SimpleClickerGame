using UnityEngine;

namespace Sources.Architecture.Interfaces
{
    public interface IService
    {
        
    }

    public interface IPopupService : IService
    {
        void ShowPopup(Vector2 position, string message);
    }

    public interface ILoaderService: IService
    {
        TResource Load<TResource>() where TResource : class, ILoadable;
    }
}