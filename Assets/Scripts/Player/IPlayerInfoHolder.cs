public interface IPlayerInfoHolder
{
    public bool IsAuthenticated { get; }
    public string PlayerName { get; }
    public string PlayerUID { get; }
}
