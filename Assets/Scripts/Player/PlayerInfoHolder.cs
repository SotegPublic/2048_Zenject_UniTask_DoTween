public class PlayerInfoHolder : IPlayerInfoHolder, IChangeblePlayerInfoHolder
{
    private bool _isAuthenticated;
    private string _playerName;
    private string _playerUID;

    public bool IsAuthenticated => _isAuthenticated;
    public string PlayerName => _playerName;
    public string PlayerUID => _playerUID;

    void IChangeblePlayerInfoHolder.SetIsAuthenticated(bool isAuth)
    {
        _isAuthenticated = isAuth;
    }

    void IChangeblePlayerInfoHolder.SetPlayerName(string name)
    {
        _playerName = name;
    }

    void IChangeblePlayerInfoHolder.SetPlayerUID(string UID)
    {
        _playerUID = UID;
    }
}
