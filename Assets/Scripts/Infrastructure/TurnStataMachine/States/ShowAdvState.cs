public class ShowAdvState : BaseTurnState<ITurnModel>, IGetAdvClosedNotification
{
    private IShowAdvSystem _advSystem;

    public ShowAdvState(ITurnModel model, IShowAdvSystem showAdvSystem) : base(model)
    {
        _advSystem = showAdvSystem;
    }

    public void OnAdvClose()
    {
        _turnModel.OnStateEnd?.Invoke(this.GetType());
    }

    public override void ProcessedState()
    {
        if (_advSystem.IsCanShowAdv())
        {
            _advSystem.ShowAdv();
        }
        else
        {
            _turnModel.OnStateEnd?.Invoke(this.GetType());
        }
    }
}
