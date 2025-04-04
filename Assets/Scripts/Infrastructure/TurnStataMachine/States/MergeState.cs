public class MergeState : BaseTurnState<IModelForMoveAndMerge>
{
    private ITileViewsPool _tilesPool;
    private IPlayerModel _playerModel;
    private ITileViewUpdater _viewUpdater;
    private ISoundManager _soundManager;

    public MergeState(IModelForMoveAndMerge model, ITileViewsPool pool, IPlayerModel playerDataModel,
        ITileViewUpdater tileViewUpdater, ISoundManager gameSoundManager) : base(model)
    {
        _tilesPool = pool;
        _playerModel = playerDataModel;
        _viewUpdater = tileViewUpdater;
        _soundManager = gameSoundManager;
    }

    public override void ProcessedState()
    {
        for (int i = 0; i < _turnModel.MoveTilesCount; i++)
        {
            if(_turnModel.MoveModels[i].IsNeedToMerge)
            {
                ProcessMerge(ref _turnModel.MoveModels[i]);
            }
        }

        _turnModel.OnStateEnd?.Invoke(this.GetType());
    }

    private void ProcessMerge(ref MoveModel moveModel)
    {
        moveModel.TargetCell.Tile.TileView.MergeParticle.Play();
        _soundManager.PlayMergeSound();

        _viewUpdater.UpdateView(moveModel.TargetCell.Tile.TileView, moveModel.TargetCell.Tile.Value);
        _playerModel.Score.Value += (int)(moveModel.TargetCell.Tile.Value * 0.5f);
        moveModel.TargetCell.Tile.ResetMergedFlag();

        _tilesPool.ReliseView(moveModel.Tile.TileView);
        moveModel.Tile.Clear();
    }
}
