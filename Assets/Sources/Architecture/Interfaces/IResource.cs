using UniRx;

namespace Sources.Architecture.Interfaces
{
    public interface IResource
    {
        IReadOnlyReactiveProperty<double> CurrentValue { get; }
        void Increase(double value);
        bool TrySpend(double value);
    }
}
