using UniRx;

namespace Sources.Architecture.Interfaces
{
    public interface IResource: IVisualData
    {
        IReadOnlyReactiveProperty<double> CurrentValue { get; }
        void Increase(double value);
        bool TrySpend(double value);
    }
}
