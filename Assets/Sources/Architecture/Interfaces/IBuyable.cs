namespace Sources.Architecture.Interfaces
{
    public interface IBuyable
    {
        double CostValue { get; }
        IResource CostResource { get; }
    }
}