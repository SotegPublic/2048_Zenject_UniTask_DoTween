using System;
using UnityEngine;

public class AwaitInputState : BaseTurnState<IChangableDirectionModel>, IDisposable
{
    private ICurrentGameStateHolder _stateHolder;
    private ISwipeInput _input;

    public AwaitInputState(IChangableDirectionModel model, ICurrentGameStateHolder gameStateHolder, ISwipeInput swipeInput) : base(model)
    {
        _stateHolder = gameStateHolder;
        _input = swipeInput;

        _input.OnSwipeEnd += Swipe;
    }

    private void Swipe(Vector2 delta)
    {
        if (_turnModel.CurrentState != this.GetType())
            return;

        if (_stateHolder.GetCurrentGameState() != typeof(GameInProgressState))
            return;

        if (delta.sqrMagnitude < 200)
            return;

        _turnModel.CurrentDir = GetDirection(delta);
        _turnModel.OnStateEnd?.Invoke(this.GetType());
    }

    private DirectionType GetDirection(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            return delta.x > 0 ? DirectionType.Left : DirectionType.Right;
        }
        else
        {
            return delta.y > 0 ? DirectionType.Down : DirectionType.Up;
        }
    }

    public override void ProcessedState()
    {
    }

    public void Dispose()
    {
        _input.OnSwipeEnd -= Swipe;
    }
}
