﻿namespace Sources.Architecture.Interfaces
{
    public interface IGenerator
    {
        IResource ProductionResource { get; }
        IResource CostResource { get; }
        int Level { get; }
        double ProductionValue { get; }
        double UpgradeCost { get; }
        float DelayTime { get; }
        void Produce();
        bool TryUpgrade();
    }
}