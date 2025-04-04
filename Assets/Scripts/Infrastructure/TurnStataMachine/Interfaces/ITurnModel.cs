using System;

public interface ITurnModel
{
    public Action<Type> OnStateEnd { get; }
}
