using System;
using System.Runtime.InteropServices;
using Zenject;

public class YandexSysetem: IYandexSystem
{
    private IYandexAdvEventsPublisher _advPublicher;

    public YandexSysetem(IYandexAdvEventsPublisher advEventsPublisher)
    {
        _advPublicher = advEventsPublisher;
    }

#if UNITY_EDITOR 
    // inject in editor events mediator
    private IYandexSystemInEditorEventsPublisher _publisher;

    [Inject]
    public void InEditorConstruct(IYandexSystemInEditorEventsPublisher yandexMediator)
    {
        _publisher = yandexMediator;
    }
#endif


    [DllImport("__Internal")]
    private static extern int CheckYandexSDK();

    [DllImport("__Internal")]
    private static extern void CheckAuth();

    [DllImport("__Internal")]
    private static extern void SetLeaderboardInfo(string leaderbordID, int value);

    [DllImport("__Internal")]
    private static extern string GetLanguage();

    [DllImport("__Internal")]
    private static extern string GetDeviceType();

    [DllImport("__Internal")]
    private static extern string GetPlayerName();

    [DllImport("__Internal")]
    private static extern string GetPlayerID();

    [DllImport("__Internal")]
    private static extern void SendReady();

    [DllImport("__Internal")]
    private static extern int YaAuth();

    [DllImport("__Internal")]
    private static extern void RateGame();

    [DllImport("__Internal")]
    private static extern IntPtr GetServerTime();

    [DllImport("__Internal")]
    private static extern void ShowAdv();

    [DllImport("__Internal")]
    private static extern void ShowRewardedAdv(int rewardID);
    [DllImport("__Internal")]
    private static extern void Debug(string text);
    [DllImport("__Internal")]
    private static extern void GetLeaderBoardInfo(string lbName, int topQuantity, bool isUserIncluded, int aroundQuantity);


    public void YandexDebug(string text)
    {
#if !UNITY_EDITOR
        Debug("Application send: " + text);
#else
        UnityEngine.Debug.LogWarning("Application send: " + text);
#endif
    }

    public bool IsYandexInit()
    {
#if !UNITY_EDITOR
        var result = CheckYandexSDK();

        return result == 1 ? true : false;
#else
        return true;
#endif
    }

    public void CheckAuthentificated()
    {
#if !UNITY_EDITOR
        CheckAuth();
#else
        _publisher.PublishAuthCheckResult(false);
#endif
    }

    public void CallAuthentification()
    {
#if !UNITY_EDITOR
        YaAuth();
#else
        _publisher.PublishAuthRequestResult(true);
#endif
    }

    public string GetLang()
    {
#if !UNITY_EDITOR
        return GetLanguage();
#else
        return "ru";
#endif
    }

    public string GetDevice()
    {
#if !UNITY_EDITOR
        return GetDeviceType();
#else
        return "desktop";
#endif
    }

    public string GetName()
    {
#if !UNITY_EDITOR
        return GetPlayerName();
#else
        return "SupaPlayer";
#endif
    }

    public string GetUID()
    {
#if !UNITY_EDITOR
        return GetPlayerID();
#else
        return "10101";
#endif
    }

    public DateTime GetTime()
    {
#if !UNITY_EDITOR
        IntPtr serverTimePtr = GetServerTime();
        string serverTimeStr = Marshal.PtrToStringAuto(serverTimePtr);

        YandexDebug(serverTimeStr);

        if (long.TryParse(serverTimeStr, out long serverTime))
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddMilliseconds(serverTime).ToLocalTime();
            return date;
        }
#endif
        return DateTime.Now;
    }

    public void SetLeaderBoardValue(string leaderboardID, int value)
    {
        YandexDebug("Send leaderbord update: " + value.ToString());
#if !UNITY_EDITOR
        SetLeaderboardInfo(leaderboardID, value);
#endif
    }

    public void UpdateLeaderBoard(string lbName, int topQuantity, bool isUserIncluded, int aroundQuantity)
    {
//#if !UNITY_EDITOR
//        GetLeaderBoardInfo(lbName, topQuantity, isUserIncluded, aroundQuantity);
//#else
//        if (TryLoadFromFile(out var lbJson))
//        {
//            GetLBInfoCallback(lbJson);
//        }
//#endif
    }

    public void SendGameReady()
    {
#if !UNITY_EDITOR
        SendReady();
#endif
    }

    public void SendRateRequest()
    {
#if !UNITY_EDITOR
        RateGame();
#endif
    }

    public void ShowAdvertising()
    {
        _advPublicher.PublishShowAdv();
#if !UNITY_EDITOR
        ShowAdv();
#else
        _publisher.PublishAdvClosed();
#endif
    }

    public void ShowRewardedAdvertising(int rewardID)
    {
        _advPublicher.PublishShowRewardedAdv(rewardID);
#if !UNITY_EDITOR
        ShowRewardedAdv(rewardID);
#else
        _publisher.PublishAdvReward(rewardID);
        _publisher.PublishRewardedAdvClosed(rewardID);
#endif
    }

    //private bool TryLoadFromFile(out string json)
    //{
    //    string fileName = "lbData";
    //    TextAsset textAsset = Resources.Load<TextAsset>(fileName);


    //    if (textAsset != null)
    //    {
    //        json = textAsset.text;
    //        return true;
    //    }
    //    else
    //    {
    //        json = null;
    //        return false;
    //    }
    //}
}