public interface IYandexCallbacksNotifier
{
    public void SendAdvNotification();
    public void SendRewardedAdvNotification(int rewardID);
    public void SendAdvClosedNotification();
    public void SendRewardedAdvClosedNotification(int rewardID);
    public void SendGetAdvRewardNotification(int rewardID);
    public void SendAuthRequestResultNotification(bool isAuth);
    public void SendAuthCheckResultNotification(bool isAuth);
    public void SendLoadExternNotification(string loadedJson);
}

#if UNITY_EDITOR
public interface IInEditorYandexCallbacksNotifier : IYandexCallbacksNotifier
{
}
#endif
