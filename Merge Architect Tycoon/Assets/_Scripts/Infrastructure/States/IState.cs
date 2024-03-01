namespace _Scripts.Infrastructure.States
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}