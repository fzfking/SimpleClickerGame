namespace Sources.Architecture.Interfaces
{
    public interface IInitiable<T> : IDeInitiable where T : IVisualData
    {
        void Init(T data);
    }
}