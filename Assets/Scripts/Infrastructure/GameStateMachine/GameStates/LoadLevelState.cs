using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Zenject;

public class LoadLevelState: BaseState
{
    private IInitable _turnStateMachine;
    private IAwaitableOnInit _gridGenerator;
    private IInitableTilesSpawner _tileSpawner;
    private IAwaitableOnInit _viewPool;
    private IPlayerModel _playerModel;
    private ISoundManager _soundManager;
    IProgressBarViewController _progressBarViewController;

    private List<UniTask> _taskList = new List<UniTask>(16);

    [Inject]
    public LoadLevelState([Inject(Id = "turnController")] IInitable stateMachine, [Inject(Id = "gridGenerator")] IAwaitableOnInit gridGenerator,
        IInitableTilesSpawner spawner, [Inject(Id = "tilesPool")] IAwaitableOnInit pool, IPlayerModel model, ISoundManager gameSoundManager,
        IProgressBarViewController uiViewController)
    {
        _turnStateMachine = stateMachine;
        this._gridGenerator = gridGenerator;
        _tileSpawner = spawner;
        _viewPool = pool;
        _playerModel = model;
        _soundManager = gameSoundManager;
        _progressBarViewController = uiViewController;
    }

    public override async void EnterState()
    {
        _progressBarViewController.FillProgressBar(0.1f);
        await _gridGenerator.Init();
        _progressBarViewController.FillProgressBar(0.1f);
        await _viewPool.Init();
        _progressBarViewController.FillProgressBar(0.1f);
        _turnStateMachine.Init();
        _tileSpawner.Init();
        _soundManager.Init();

        await SpawnTiles();
        _progressBarViewController.FillProgressBar(0.2f);

        OnStateEnd?.Invoke(this.GetType());
    }

    public override void ExitState()
    {
        _soundManager.PlayBackground();
        _progressBarViewController.HideUI();
    }

    private async UniTask SpawnTiles()
    {
        if(_playerModel.TilesOnField.Count > 0)
        {
            _taskList.Clear();

            for(int i = 0; i < _playerModel.TilesOnField.Count; i++)
            {
                _taskList.Add(_tileSpawner.SpawnTile(_playerModel.TilesOnField[i].Value, _playerModel.TilesOnField[i].CellID));
            }

            await UniTask.WhenAll(_taskList);
        }
        else
        {

            await _tileSpawner.SpawnTile();
            await _tileSpawner.SpawnTile();
        }
    }
}
