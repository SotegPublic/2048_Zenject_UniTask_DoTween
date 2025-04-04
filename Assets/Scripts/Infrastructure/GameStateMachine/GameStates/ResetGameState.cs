using System;
using UnityEngine;
using Zenject;

public class ResetGameState : BaseState, IDisposable
{
    private IGridHolder _gridHolder;
    private ITileViewsPool _viewsPool;
    private ISaveSystem _saveSystem;
    private IInitableTilesSpawner _tileSpawner;
    private IPlayerModel _playerModel;
    private IEndScreenController _uiViewController;
    private ICurrentGameStateHolder _stateHolder;
    private IEndGameChecker _endGameChecker;

    public bool isGameOver;

    [Inject]
    public ResetGameState(IGridHolder gameGrdHolder, ITileViewsPool tileViewsPool, ISaveSystem gameSaveSystem, IEndGameChecker endChecker,
        IInitableTilesSpawner spawner, IPlayerModel model, IEndScreenController endScreenController, ICurrentGameStateHolder gameStateHolder)
    {
        _gridHolder = gameGrdHolder;
        _viewsPool = tileViewsPool;
        _saveSystem = gameSaveSystem;
        _tileSpawner = spawner;
        _playerModel = model;
        _endGameChecker = endChecker;
        _uiViewController = endScreenController;
        _stateHolder = gameStateHolder;

        _endGameChecker.OnGameOver += UpEndGameFlag;
        _uiViewController.OnEndScreenButtonClick += ResetGame;
    }

    private void UpEndGameFlag()
    {
        isGameOver = true;
    }

    private async void ResetGame()
    {
        if (_stateHolder.GetCurrentGameState() != this.GetType())
            return;
        
        _uiViewController.HideEndScreen();

        for (int i = 0; i < _gridHolder.Cells.GetLength(0); i++)
        {
            var cell = _gridHolder.Cells[i, 0];

            while (cell != null)
            {
                if (cell.IsBusy)
                {
                    var tile = cell.Tile.TileView;
                    _viewsPool.ReliseView(tile);

                    cell.ClearCell();
                }

                cell = cell.GetNeighbour(DirectionType.Down);
            }
        }

        _playerModel.Score.Value = 0;

        await _tileSpawner.SpawnTile();
        await _tileSpawner.SpawnTile();

        _saveSystem.Save();

        OnStateEnd?.Invoke(this.GetType());
    }

    public override void EnterState()
    {
        if(isGameOver)
        {
            _uiViewController.ShowEndScreen(false);
        }
        else
        {
            ResetGame();
        }
    }

    public override void ExitState()
    {
        isGameOver = false;
    }

    public void Dispose()
    {
        _endGameChecker.OnGameOver -= UpEndGameFlag;
        _uiViewController.OnEndScreenButtonClick -= ResetGame;
    }
}
