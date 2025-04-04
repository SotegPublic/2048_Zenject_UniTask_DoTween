public interface IModelForMoveAndMerge : ITurnModel
{
    public MoveModel[] MoveModels { get; }
    public int MoveTilesCount { get; }
}
