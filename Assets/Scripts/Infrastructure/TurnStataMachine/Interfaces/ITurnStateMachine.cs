public interface ITurnStateMachine
{
    public void Start();

    public bool IsTurnInProgress { get; }
}
