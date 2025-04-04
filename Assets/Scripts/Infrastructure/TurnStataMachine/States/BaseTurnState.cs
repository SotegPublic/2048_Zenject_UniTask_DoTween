public abstract class BaseTurnState<T> : ITurnState where T : ITurnModel
{
    protected T _turnModel;

    public BaseTurnState(T model)
    {
        _turnModel = model;
    }

    public abstract void ProcessedState();
}
