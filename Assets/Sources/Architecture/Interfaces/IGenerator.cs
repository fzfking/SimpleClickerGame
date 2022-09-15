﻿using UniRx;
using Unity.Plastic.Newtonsoft.Json.Serialization;

namespace Sources.Architecture.Interfaces
{
    public interface IGenerator: IVisualData, IProgressive, IGeneratorProgress, IBuyable
    {
        IResource ProductionResource { get; }
        IReadOnlyReactiveProperty<int> Level { get; }
        double ProductionValue { get; }
        float DelayTime { get; }
        void TryProduce();
        bool CanUpgrade(int levelValue);
        bool TryUpgrade();
    }
}