using UnityEngine;

namespace Sources.Architecture.Interfaces
{
    public interface IVisualData
    {
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
    }
}
