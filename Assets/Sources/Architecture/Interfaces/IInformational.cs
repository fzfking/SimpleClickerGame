using System;

namespace Sources.Architecture.Interfaces
{
    public interface IInformational
    {
        event Action<IVisualData> InfoNeeded;
    }
}