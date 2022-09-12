using UniRx;

namespace Sources.Architecture.Interfaces
{
    public interface IGenerator: IVisualData, IProgressive
    {
        IResource ProductionResource { get; }
        IResource CostResource { get; }
        IReadOnlyReactiveProperty<int> Level { get; }
        double ProductionValue { get; }
        double UpgradeCost { get; }
        float DelayTime { get; }
        void Produce();
        bool CanUpgrade(int levelValue);
        bool TryUpgrade();
    }
}