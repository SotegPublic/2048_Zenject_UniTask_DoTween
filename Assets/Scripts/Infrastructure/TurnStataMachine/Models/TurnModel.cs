using System;

public class TurnModel : ITurnModel, IModelForMoveCalculations, IModelForMoveAndMerge, IModelForSpawn, IChangableDirectionModel
{
    public MoveModel[] MoveModels { get; set; }
    public DirectionType CurrentDir { get; set; }
    public Type CurrentState { get; set; }
    public Action<Type> OnStateEnd { get; set; }
    public bool IsNeedToMove { get; set; }
    public int MoveTilesCount { get; set; }
    public bool IsNoEmptyCell { get; set; }
}
