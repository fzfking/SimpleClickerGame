using UniRx;

namespace Sources.Architecture.Interfaces
{
    public interface IGenerator: IVisualData, IProgressive, IGeneratorProgress, IBuyable, ISaveable
    {
        IResource ProductionResource { get; }
        IReadOnlyReactiveProperty<int> Level { get; }
        double ProductionValue { get; }
        float DelayTime { get; }
        void TryProduce();
        bool CanUpgrade(int levelValue);
        bool TryUpgrade(int levels);
        double GetCost(int levels);
    }
}