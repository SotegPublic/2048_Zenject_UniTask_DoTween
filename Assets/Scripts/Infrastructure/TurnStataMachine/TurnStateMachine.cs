using System;
using System.Threading;
using Zenject;
using System.Collections.Generic;

public class TurnStateMachine: IInitable, ITurnStateMachine, IDisposable
{
    private IGridHolder _gridHolder;
    private ITilesSpawner _tilesSpawner;
    private ISaveSystem _saveSystem;
    private IEndGameChecker _endChecker;
    private ITurnStateFactory _statesFactory;

    private bool _isTurnInProgress;
    private TurnModel _turnModel;

    public bool IsTurnInProgress => _isTurnInProgress;

    private Dictionary<Type, ITurnState> gameStates = new Dictionary<Type, ITurnState>(8);

    [Inject]
    public TurnStateMachine(IGridHolder gameGridHolder, ITilesSpawner spawner,
        ISaveSystem gameSaveSystem, IEndGameChecker endGameChecker, ITurnStateFactory factory, TurnModel model)
    {
        _gridHolder = gameGridHolder;
        _saveSystem = gameSaveSystem;
        _endChecker = endGameChecker;
        _tilesSpawner = spawner;
        _statesFactory = factory;
        _turnModel = model;
    }


    public void Init()
    {
        _turnModel.MoveModels = new MoveModel[_gridHolder.Cells.GetLength(0) * _gridHolder.Cells.GetLength(0)];
        _turnModel.OnStateEnd += NextStep;

        gameStates.Add(typeof(AwaitInputState), _statesFactory.CreateState<AwaitInputState>());
        gameStates.Add(typeof(ShowAdvState), _statesFactory.CreateState<ShowAdvState>());

        var calcState = _statesFactory.CreateState<CalculateMoveState>();
        calcState.Init();
        gameStates.Add(typeof(CalculateMoveState), calcState);

        gameStates.Add(typeof(MoveState), _statesFactory.CreateState<MoveState>());
        gameStates.Add(typeof(MergeState), _statesFactory.CreateState<MergeState>());
        gameStates.Add(typeof(SpawTileState), _statesFactory.CreateState<SpawTileState>());
        gameStates.Add(typeof(CheckWinState), _statesFactory.CreateState<CheckWinState>());
    }

    public void Start()
    {
        if(_turnModel.CurrentState != typeof(AwaitInputState))
            _turnModel.CurrentState = typeof(AwaitInputState);
    }

    private void NextStep(Type currentStateType)
    {
        if (_turnModel.CurrentState != currentStateType)
            return;

        switch (currentStateType)
        {
            case var _ when currentStateType == typeof(AwaitInputState):
                _isTurnInProgress = true;

                _turnModel.CurrentState = typeof(ShowAdvState);
                gameStates[typeof(ShowAdvState)].ProcessedState();
                break;
            case var _ when currentStateType == typeof(ShowAdvState):
                _turnModel.CurrentState = typeof(CalculateMoveState);
                gameStates[typeof(CalculateMoveState)].ProcessedState();
                break;
            case var _ when currentStateType == typeof(CalculateMoveState):

                if(_turnModel.IsNeedToMove)
                {
                    _turnModel.CurrentState = typeof(MoveState);
                    gameStates[typeof(MoveState)].ProcessedState();
                }
                else
                {
                    _turnModel.CurrentState = typeof(AwaitInputState);
                    _isTurnInProgress = false;
                }

                break;
            case var _ when currentStateType == typeof(MoveState):
                _turnModel.CurrentState = typeof(MergeState);
                gameStates[typeof(MergeState)].ProcessedState();
                break;
            case var _ when currentStateType == typeof(MergeState):
                _turnModel.CurrentState = typeof(SpawTileState);
                gameStates[typeof(SpawTileState)].ProcessedState();
                break;
            case var _ when currentStateType == typeof(SpawTileState):
                _turnModel.CurrentState = typeof(CheckWinState);
                gameStates[typeof(CheckWinState)].ProcessedState();
                _saveSystem.Save();
                break;
            case var _ when currentStateType == typeof(CheckWinState):

                if (_tilesSpawner.IsNoEmptyCell)
                {
                    _endChecker.GameOverCheck();
                }
                
                _turnModel.CurrentState = typeof(AwaitInputState);
                gameStates[typeof(AwaitInputState)].ProcessedState();
                _isTurnInProgress = false;

                break;
            default:
                break;
        }
    }

    public void Dispose()
    {
        _turnModel.OnStateEnd -= NextStep;
        gameStates.Clear();
    }
}