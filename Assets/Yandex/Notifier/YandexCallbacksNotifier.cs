using System;
using System.Collections.Generic;
using Zenject;

public partial class YandexCallbacksNotifier: IYandexCallbacksNotifier, IDisposable
{
    private IYandexAdvEventsSubscriber _advEventsSubscriber;
    private List<IGetAdvNotification> _advRecipients;
    private List<IGetRewardedAdvNotification> _rewardedAdvRecipients;
    private List<IGetAdvClosedNotification> _advClosedRecipients;
    private List<IGetRewardedAdvClosedNotification> _rewardedAdvClosedRecipients;
    private List<IGetAdvRewardNotification> _getRewardRecipients;
    private List<IGetAuthRequestResultNotification> _authRequestResultRecipients;
    private IGetAuthCheckResultNotification _authCheckResultRecipient;
    private IGetLoadExternNotification _loadExternResultRecipient;

    public YandexCallbacksNotifier(IYandexAdvEventsSubscriber advEventsSubscriber, List<IGetAdvNotification> advNotificationRecipients, List<IGetRewardedAdvNotification> rewardedAdvNotificationRecipients,
        List<IGetAdvClosedNotification> advClosedNotificationRecipients, List<IGetRewardedAdvClosedNotification> rewardedAdvClosedNotificationRecipients,
        List<IGetAdvRewardNotification> getRewardNotificationRecipients, List<IGetAuthRequestResultNotification> authRequestNotificationRecipients,
        IGetAuthCheckResultNotification authCheckNotificationRecipient, IGetLoadExternNotification loadExternNotificationRecipient)
    {
        _advEventsSubscriber = advEventsSubscriber;
        _advRecipients = advNotificationRecipients;
        _rewardedAdvRecipients = rewardedAdvNotificationRecipients;
        _advClosedRecipients = advClosedNotificationRecipients;
        _rewardedAdvClosedRecipients = rewardedAdvClosedNotificationRecipients;
        _getRewardRecipients = getRewardNotificationRecipients;
        _authCheckResultRecipient = authCheckNotificationRecipient;
        _authRequestResultRecipients = authRequestNotificationRecipients;
        _loadExternResultRecipient = loadExternNotificationRecipient;

        _advEventsSubscriber.SubscribeToShowAdv(SendAdvNotification);
        _advEventsSubscriber.SubscribeToShowRewardedAdv(SendRewardedAdvNotification);
    }

    public void SendAdvNotification()
    {
        for(int i = 0; i < _advRecipients.Count; i++)
        {
            _advRecipients[i].OnAdvShow();
        }
    }

    public void SendRewardedAdvNotification(int rewardID)
    {
        for (int i = 0; i < _rewardedAdvRecipients.Count; i++)
        {
            _rewardedAdvRecipients[i].OnRewardedAdvShow(rewardID);
        }
    }

    public void SendAdvClosedNotification()
    {
        for (int i = 0; i < _advClosedRecipients.Count; i++)
        {
            _advClosedRecipients[i].OnAdvClose();
        }
    }

    public void SendRewardedAdvClosedNotification(int rewardID)
    {
        for (int i = 0; i < _rewardedAdvClosedRecipients.Count; i++)
        {
            _rewardedAdvClosedRecipients[i].OnRewardedAdvClose(rewardID);
        }
    }

    public void SendGetAdvRewardNotification(int rewardID)
    {
        for (int i = 0; i < _getRewardRecipients.Count; i++)
        {
            _getRewardRecipients[i].GetAdvReward(rewardID);
        }
    }

    public void SendAuthRequestResultNotification(bool isAuth)
    {
        for (int i = 0; i < _authRequestResultRecipients.Count; i++)
        {
            _authRequestResultRecipients[i].GetAuthRequestResult(isAuth);
        }
    }

    public void SendAuthCheckResultNotification(bool isAuth)
    {
        _authCheckResultRecipient.GetAuthCheckResult(isAuth);
    }

    public void SendLoadExternNotification(string loadedJson)
    {
        _loadExternResultRecipient.GetLoadExtern(loadedJson);
    }

    public void Dispose()
    {
        _advEventsSubscriber.UnsubscribeFromShowAdv(SendAdvNotification);
        _advEventsSubscriber.UnsubscribeFromShowRewardedAdv(SendRewardedAdvNotification);

#if UNITY_EDITOR
        DisposeInEditorPart();
#endif
    }
}

