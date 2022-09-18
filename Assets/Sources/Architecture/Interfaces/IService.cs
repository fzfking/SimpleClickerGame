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

    public interface IInformationService : IService
    {
        void ShowInfo(IVisualData data);
    }

    public interface ILoaderService: IService
    {
        TResource Load<TResource>() where TResource : class, ILoadable;
    }

    public interface IBuyService : IService
    {
        void Enable();
        void GoToNextAmount();
        void ChangeAmount(BuyAmount amount);
        string GetCurrentAmount();
    }

    public interface IProgressSaverService : IService
    {
        void Save<TData>(TData data) where TData : IVisualData;
    }

    public interface IProgressLoaderService : IService
    {
        IProgressContainer Load<TData>(string name) where TData : IVisualData;
    }
}