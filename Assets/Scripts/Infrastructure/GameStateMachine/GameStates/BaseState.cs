using System;

public abstract class BaseState: IGameState
{
    public Action<Type> OnStateEnd { get; set; }

    public abstract void EnterState();

    public abstract void ExitState();
}
