public interface ITurnStateFactory
{
    public T CreateState<T>() where T : ITurnState;
}
