using System;

public class PlayerRecordSystem : IDisposable
{
    private IPlayerModel _playerModel;
    private IYandexSystem _yandex;
    private ICurrentGameStateHolder _currentGameStateHolder;

    private DateTime _lastLBUpdateTime;

    private const string LB_NAME = "Record";
    private const int LB_UPDATE_DELAY = 1;

    public PlayerRecordSystem(IPlayerModel model, IYandexSystem yandexSystem, ICurrentGameStateHolder currentGameStateHolder)
    {
        _playerModel = model;
        _currentGameStateHolder = currentGameStateHolder;

        _playerModel.Score.Subscribe(CheckRecord);
        _yandex = yandexSystem;
    }

    private void CheckRecord(int newValue)
    {
        if (_currentGameStateHolder.GetCurrentGameState() != typeof(GameInProgressState))
            return;
        
        if (newValue <= _playerModel.Record.Value)
            return;

        _playerModel.Record.Value = newValue;

        var currentTime = _yandex.GetTime();
        var dif = currentTime - _lastLBUpdateTime;

        if (dif.TotalSeconds < LB_UPDATE_DELAY)
            return;

        _yandex.SetLeaderBoardValue(LB_NAME, newValue);
        _lastLBUpdateTime = currentTime;
    }
    public void Dispose()
    {
        _playerModel.Score.Unsubscribe(CheckRecord);
    }

}
