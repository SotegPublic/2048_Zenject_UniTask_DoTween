using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

public class GameInProgressState : BaseState, IDisposable
{
    private IEndGameChecker _endGameChecker;
    private IInGameUIViewController _inGameViewUIController;
    private ITurnStateMachine _turnStateMachine;
    private ICurrentGameStateHolder _currentStateHolder;

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    [Inject]
    public GameInProgressState(IEndGameChecker endChecker, IInGameUIViewController uiResetableController, ITurnStateMachine stateMachine, ICurrentGameStateHolder currentGameStateHolder)
    {
        _endGameChecker = endChecker;
        _inGameViewUIController = uiResetableController;
        _turnStateMachine = stateMachine;
        _currentStateHolder = currentGameStateHolder;


        _endGameChecker.OnGameOver += GameOver;
        _inGameViewUIController.OnResetButtonClick += GameOver;
    }

    private async void GameOver()
    {
        if (_currentStateHolder.GetCurrentGameState() != this.GetType())
            return;
        
        await UniTask.WaitUntil(IsTurnEnd, cancellationToken: _cancellationTokenSource.Token).SuppressCancellationThrow();

        OnStateEnd?.Invoke(this.GetType());
    }

    private bool IsTurnEnd()
    {
        return !_turnStateMachine.IsTurnInProgress;
    }

    public override void EnterState()
    {
        _inGameViewUIController.Init();
        _inGameViewUIController.ShowUI();
        _turnStateMachine.Start();
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource.Dispose();

        _endGameChecker.OnGameOver -= GameOver;
        _inGameViewUIController.OnResetButtonClick -= GameOver;
    }

    public override void ExitState()
    {
    }
}
