using GameAnalyticsSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameAnalyticsSystem : IInitializable, IGameAnalyticsSystem
{

    private string _sessionID;

    public void Initialize()
    {
        GameAnalytics.Initialize();
        _sessionID = Guid.NewGuid().ToString();
        SendFunnelEvent("InitSystem");
    }

    public void SendFunnelEvent(string stageName)
    {
        GameAnalytics.NewDesignEvent($"Funnel:{stageName}:{_sessionID}");
    }

    public void SendInterAdvEvent()
    {
        GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Interstitial, "Yandex", "FullScreen");
    }
}

public interface IGameAnalyticsSystem 
{
    public void SendFunnelEvent(string stageName);
    public void SendInterAdvEvent();
}
