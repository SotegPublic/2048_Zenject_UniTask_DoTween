using System;

public interface IGameState
{
    public Action<Type> OnStateEnd { get; set; }
    public void EnterState();

    public void ExitState();
}
