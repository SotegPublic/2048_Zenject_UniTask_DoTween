using System;

public class CurrentGameStateHolder: ICurrentGameStateHolder, IChangableGameStateHolder
{
    private Type _currentGameState;

    public Type GetCurrentGameState() => _currentGameState;

    void IChangableGameStateHolder.ChangeCurrentGameState(Type newStateType)
    {
        _currentGameState = newStateType;
    }

}
