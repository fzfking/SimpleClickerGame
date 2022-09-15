namespace Sources.Architecture.Interfaces
{
    public interface IManager: IVisualData, IBuyable
    {
        IGenerator Generator { get; }
        bool IsActive { get; }
        void ChangeActive(bool value);
    }
}