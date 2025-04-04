using System;

public interface IGameStateMachine
{
    public void EndCurrentState(Type currentStateType);
    public void StartGame();
}
