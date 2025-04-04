using System;

public interface IChangableGameStateHolder: ICurrentGameStateHolder
{
    void ChangeCurrentGameState(Type newStateType);
}
