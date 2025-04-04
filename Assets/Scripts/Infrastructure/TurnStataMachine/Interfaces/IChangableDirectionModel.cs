using System;

public interface IChangableDirectionModel : ITurnModel
{
    public DirectionType CurrentDir { get; set; }
    public Type CurrentState { get; }
}
