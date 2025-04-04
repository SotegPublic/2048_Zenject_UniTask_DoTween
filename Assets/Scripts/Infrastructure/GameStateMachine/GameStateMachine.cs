using System;
using System.Collections.Generic;
using Zenject;

public class GameStateMachine : IGameStateMachine, IDisposable
{
    private IChangableGameStateHolder _gameStateHolder;
    private IStateFactory _statesFactory;
    private IGameAnalyticsSystem _analytics;

    private Dictionary<Type, IGameState> gameStates = new Dictionary<Type, IGameState>(8);

    [Inject]
    public GameStateMachine(IChangableGameStateHolder stateHolder, IStateFactory gamseStatesFactory, IGameAnalyticsSystem gameAnalytics)
    {
        _gameStateHolder = stateHolder;
        _statesFactory = gamseStatesFactory;
        _analytics = gameAnalytics;
    }

    [Inject]
    public void Init()
    {
        gameStates.Add(typeof(InitYandexState), _statesFactory.CreateState<InitYandexState>());
        gameStates.Add(typeof(PlayerAuthState), _statesFactory.CreateState<PlayerAuthState>());
        gameStates.Add(typeof(LoadPlayerState), _statesFactory.CreateState<LoadPlayerState>());
        gameStates.Add(typeof(LoadLevelState), _statesFactory.CreateState<LoadLevelState>());
        gameStates.Add(typeof(GameInProgressState), _statesFactory.CreateState<GameInProgressState>());
        gameStates.Add(typeof(ResetGameState), _statesFactory.CreateState<ResetGameState>());

        foreach(var state in gameStates.Values)
        {
            state.OnStateEnd += EndCurrentState;
        }
    }

    public void EndCurrentState(Type currentStateType)
    {
        if (_gameStateHolder.GetCurrentGameState() != currentStateType)
            return;

        gameStates[currentStateType].ExitState();

        switch (currentStateType)
        {
            case var _ when currentStateType == typeof(InitYandexState):
                _gameStateHolder.ChangeCurrentGameState(typeof(PlayerAuthState));
                SendFunnelEvent<PlayerAuthState>();
                gameStates[typeof(PlayerAuthState)].EnterState();
                break;
            case var _ when currentStateType == typeof(PlayerAuthState):
                _gameStateHolder.ChangeCurrentGameState(typeof(LoadPlayerState));
                SendFunnelEvent<LoadPlayerState>();
                gameStates[typeof(LoadPlayerState)].EnterState();
                break;
            case var _ when currentStateType == typeof(LoadPlayerState):
                _gameStateHolder.ChangeCurrentGameState(typeof(LoadLevelState));
                SendFunnelEvent<LoadLevelState>();
                gameStates[typeof(LoadLevelState)].EnterState();
                break;
            case var _ when currentStateType == typeof(LoadLevelState):
                _gameStateHolder.ChangeCurrentGameState(typeof(GameInProgressState));
                SendFunnelEvent<GameInProgressState>();
                gameStates[typeof(GameInProgressState)].EnterState();
                break;
            case var _ when currentStateType == typeof(GameInProgressState):
                _gameStateHolder.ChangeCurrentGameState(typeof(ResetGameState));
                gameStates[typeof(ResetGameState)].EnterState();
                break;
            case var _ when currentStateType == typeof(ResetGameState):
                _gameStateHolder.ChangeCurrentGameState(typeof(GameInProgressState));
                gameStates[typeof(GameInProgressState)].EnterState();
                break;
            default:
                break;
        }
    }

    public void StartGame()
    {
        _gameStateHolder.ChangeCurrentGameState(typeof(InitYandexState));
        gameStates[typeof(InitYandexState)].EnterState();
        SendFunnelEvent<InitYandexState>();
    }

    private void SendFunnelEvent<T>() where T : IGameState
    {
        var stateName = nameof(T);
        _analytics.SendFunnelEvent(stateName);
    }

    public void Dispose()
    {
        foreach (var state in gameStates.Values)
        {
            state.OnStateEnd -= EndCurrentState;
        }
    }
}
