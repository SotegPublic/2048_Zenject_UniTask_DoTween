public interface IModelForMoveCalculations : ITurnModel
{
    public DirectionType CurrentDir { get; }
    public MoveModel[] MoveModels { get;}
    public int MoveTilesCount { get; set; }
    public bool IsNeedToMove { get; set; }
}
