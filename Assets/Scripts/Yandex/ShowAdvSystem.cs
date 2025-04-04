using System;

public class ShowAdvSystem : IShowAdvSystem
{
    private IYandexSystem _yandex;
    private IGameAnalyticsSystem _gameAnalytics;

    private DateTime _lastShowingAdvTime;

    private const int SHOW_ADV_DELAY = 60;

    public ShowAdvSystem(IYandexSystem yandexSystem, IGameAnalyticsSystem analytics)
    {
        _yandex = yandexSystem;
        _gameAnalytics = analytics;
    }

    public bool IsCanShowAdv()
    {
        var currentTime = _yandex.GetTime();
        var dif = currentTime - _lastShowingAdvTime;

        if(dif.TotalSeconds < SHOW_ADV_DELAY)
            return false;
        
        return true;
    }

    public void ShowAdv()
    {
        _lastShowingAdvTime = _yandex.GetTime();
        _yandex.ShowAdvertising();
        _gameAnalytics.SendInterAdvEvent();
    }
}
