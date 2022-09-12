using UniRx;

namespace Sources.Architecture.Interfaces
{
    public interface IProgressive
    {
        IReadOnlyReactiveProperty<float> Progress { get; }
    }
}