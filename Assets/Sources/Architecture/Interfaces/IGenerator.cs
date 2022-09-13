using UniRx;
using Unity.Plastic.Newtonsoft.Json.Serialization;

namespace Sources.Architecture.Interfaces
{
    public interface IGenerator: IVisualData, IProgressive, IGeneratorProgress
    {
        IResource ProductionResource { get; }
        IResource CostResource { get; }
        IReadOnlyReactiveProperty<int> Level { get; }
        double ProductionValue { get; }
        double UpgradeCost { get; }
        float DelayTime { get; }
        void TryProduce();
        bool CanUpgrade(int levelValue);
        bool TryUpgrade();
    }
}