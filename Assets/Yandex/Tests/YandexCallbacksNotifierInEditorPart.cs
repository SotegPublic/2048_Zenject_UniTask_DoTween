#if UNITY_EDITOR
using System;
using Zenject;

public partial class YandexCallbacksNotifier : IInEditorYandexCallbacksNotifier
{
    private ILoadMediator _loadMediator;
    private IYandexSystemInEditorSubscriber _yandexMediator;

    [Inject]
    public void InjectTestMediators(ILoadMediator subscribableLoadMediator, IYandexSystemInEditorSubscriber subscribaleYandexMediator)
    {
        _loadMediator = subscribableLoadMediator;
        _loadMediator.OnJsonLoad += SendLoadExternNotification;

        _yandexMediator = subscribaleYandexMediator;

        _yandexMediator.SubscribeToAdvClosed(SendAdvClosedNotification);
        _yandexMediator.SubscribeToAuthCheck(SendAuthCheckResultNotification);
        _yandexMediator.SubscribeToAuthRequest(SendAuthRequestResultNotification);
        _yandexMediator.SubscribeToAdvReward(SendGetAdvRewardNotification);
        _yandexMediator.SubscribeToRewardedAdvClosed(SendRewardedAdvClosedNotification);
    }

    private void DisposeInEditorPart()
    {
        _loadMediator.OnJsonLoad -= SendLoadExternNotification;

        _yandexMediator.UnsubscribeFromAdvClosed(SendAdvClosedNotification);
        _yandexMediator.UnsubscribeFromAuthCheck(SendAuthCheckResultNotification);
        _yandexMediator.UnsubscribeFromAuthRequest(SendAuthRequestResultNotification);
        _yandexMediator.UnsubscribeFromAdvReward(SendGetAdvRewardNotification);
        _yandexMediator.UnsubscribeFromRewardedAdvClosed(SendRewardedAdvClosedNotification);
    }
}
#endif