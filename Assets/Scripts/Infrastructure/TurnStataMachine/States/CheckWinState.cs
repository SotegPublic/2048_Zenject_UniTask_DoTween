using System;
using UnityEditor;

public class CheckWinState : BaseTurnState<ITurnModel>, IDisposable
{
    private IResettingEndScreenController _uiController;
    private ICurrentGameStateHolder _stateHolder;
    private IGridHolder _gridHolder;
    private IEndGameChecker _endGameChecker;

    private bool _isWinInSession;

    private const int WIN_VALUE = 2048;

    public CheckWinState(IModelForSpawn model, IResettingEndScreenController endScreenController, ICurrentGameStateHolder gameStateHolder,
        IGridHolder gameGridHolder, IEndGameChecker endChecker) : base(model)
    {
        _uiController = endScreenController;
        _stateHolder = gameStateHolder;
        _gridHolder = gameGridHolder;
        _endGameChecker = endChecker;

        _uiController.OnEndScreenButtonClick += ContinueClick;

        _uiController.OnResetButtonClick += ResetSession;
        _endGameChecker.OnGameOver += ResetSession;
    }

    private void ResetSession()
    {
        _isWinInSession = false;
    }

    private void ContinueClick()
    {
        if (_stateHolder.GetCurrentGameState() != typeof(GameInProgressState))
            return;

        _uiController.HideEndScreen();
        _turnModel.OnStateEnd?.Invoke(this.GetType());
    }

    public override void ProcessedState()
    {
        if(_isWinInSession)
        {
            _turnModel.OnStateEnd?.Invoke(this.GetType());
        }
        else
        {
            if (CheckIsWin())
            {
                _isWinInSession = true;
                _uiController.ShowEndScreen(true);
            }
            else
            {
                _turnModel.OnStateEnd?.Invoke(this.GetType());
            }
        }
    }

    private bool CheckIsWin()
    {
        for (int i = 0; i < _gridHolder.Cells.GetLength(0); i++)
        {
            var cell = _gridHolder.Cells[i, 0];

            while (cell != null)
            {
                if (cell.IsBusy)
                {
                    if (cell.Tile.Value >= WIN_VALUE)
                        return true;
                }

                cell = cell.GetNeighbour(DirectionType.Down);
            }
        }

        return false;
    }

    public void Dispose()
    {
        _uiController.OnEndScreenButtonClick -= ContinueClick;

        _uiController.OnResetButtonClick -= ResetSession;
        _endGameChecker.OnGameOver -= ResetSession;
    }
}