using System;

public class YandexAdvMediator : IYandexAdvEventsPublisher, IYandexAdvEventsSubscriber
{
    private event Action<int> _onShowRewardedAdv;
    private event Action _onShowAdv;

    public void PublishShowAdv() => _onShowAdv?.Invoke();
    public void PublishShowRewardedAdv(int rewardId) => _onShowRewardedAdv?.Invoke(rewardId);

    public void SubscribeToShowAdv(Action handler) => _onShowAdv += handler;
    public void UnsubscribeFromShowAdv(Action handler) => _onShowAdv -= handler;

        public void SubscribeToShowRewardedAdv(Action<int> handler) => _onShowRewardedAdv += handler;
    public void UnsubscribeFromShowRewardedAdv(Action<int> handler) => _onShowRewardedAdv -= handler;
}
