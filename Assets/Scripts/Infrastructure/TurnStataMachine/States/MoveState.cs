using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class MoveState : BaseTurnState<IModelForMoveAndMerge>
{
    private float _moveSpeed;

    private List<UniTask> _taskList = new List<UniTask>(16);

    public MoveState(IModelForMoveAndMerge model, float speed) : base(model)
    {
        _moveSpeed = speed;
    }

    public override async void ProcessedState()
    {
        _taskList.Clear();
        for (int i = 0; i < _turnModel.MoveTilesCount; i++)
        {
            _taskList.Add(MoveTile(_turnModel.MoveModels[i]));
        }

        await UniTask.WhenAll(_taskList);
        _turnModel.OnStateEnd?.Invoke(this.GetType());
    }

    private async UniTask MoveTile(MoveModel moveModel)
    {
        await moveModel.Tile.TileTransform
            .DOMove(moveModel.TargetCell.Position, _moveSpeed * moveModel.PathLenth)
            .SetLink(moveModel.Tile.TileView.gameObject)
            .ToUniTask();
    }
}
