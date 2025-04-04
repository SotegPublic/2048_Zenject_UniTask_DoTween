using System;
using UnityEngine;

public class AwaitInputState : BaseTurnState<IChangableDirectionModel>, IDisposable
{
    private ICurrentGameStateHolder _stateHolder;
    private ISwipeInput _input;

    private bool _isInSwipeProcess;

    public AwaitInputState(IChangableDirectionModel model, ICurrentGameStateHolder gameStateHolder, ISwipeInput swipeInput) : base(model)
    {
        _stateHolder = gameStateHolder;
        _input = swipeInput;

        _input.OnSwipe += Swipe;
        _input.OnSwipeEnd += WhenSwipeEnd;
    }

    private void Swipe(Vector2 delta)
    {
        if (_isInSwipeProcess)
            return;

        if (_turnModel.CurrentState != this.GetType())
            return;

        if (_stateHolder.GetCurrentGameState() != typeof(GameInProgressState))
            return;

        if (delta.sqrMagnitude < 2000)
            return;

        _isInSwipeProcess = true;
        _turnModel.CurrentDir = GetDirection(delta);
        _turnModel.OnStateEnd?.Invoke(this.GetType());
    }

    private void WhenSwipeEnd()
    {
        _isInSwipeProcess = false;
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
        _input.OnSwipe -= Swipe;
        _input.OnSwipeEnd -= WhenSwipeEnd;
    }
}
