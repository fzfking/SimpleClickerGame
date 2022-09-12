namespace Sources.Architecture.Interfaces
{
    public interface IPayloadableState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }
    public interface IState: IExitableState
    {
        void Enter();
    }

    public interface IExitableState
    {
        void Exit();
    }
}