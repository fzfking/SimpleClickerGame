namespace Sources.Architecture.Interfaces
{
    public interface IInitiable<T> where T: IVisualData
    {
        void Init(T data);
        void DeInit();
    }
}