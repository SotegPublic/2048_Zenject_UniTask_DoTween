using UnityEngine;
using Zenject;

public class YandexReceiver : MonoBehaviour
{
    private IYandexCallbacksNotifier _notifier;

    [Inject]
    public void Construct(IYandexCallbacksNotifier yandexNotifier)
    {
        _notifier = yandexNotifier;
    }

    public void AuthCallback(int value)
    {
        var result = value == 1 ? true : false;

        _notifier.SendAuthRequestResultNotification(result);
    }

    public void ShowAdvCallback()
    {
        _notifier.SendAdvClosedNotification();
    }

    public void ShowRewardedAdvCallback(int rewardID)
    {
        _notifier.SendRewardedAdvClosedNotification(rewardID);
    }

    public void GetAdvRewardCallback(int rewardID)
    {
        _notifier.SendGetAdvRewardNotification(rewardID);
    }

    public void LoadExternCallback(string jsonData)
    {
        _notifier.SendLoadExternNotification(jsonData);
    }

    public void AfterAuthCheckCallback(int result)
    {
        var isAuth = result == 1 ? true : false;

        _notifier.SendAuthCheckResultNotification(isAuth);
    }

    public void GetLBInfoCallback(string lbJson)
    {
        //notifier.SendUpdateLBNotification(lbJson);
    }
}
