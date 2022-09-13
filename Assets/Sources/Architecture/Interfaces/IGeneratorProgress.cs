using System;
using UniRx;

namespace Sources.Architecture.Interfaces
{
    public interface IGeneratorProgress
    {
        ReactiveCommand<double> OnEnd { get; }
    }
}