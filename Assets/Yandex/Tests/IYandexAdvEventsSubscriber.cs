using System;

public interface IYandexAdvEventsSubscriber
{
    // Реклама
    void SubscribeToShowAdv(Action handler);
    void UnsubscribeFromShowAdv(Action handler);

    // Rewarded реклама
    void SubscribeToShowRewardedAdv(Action<int> handler);
    void UnsubscribeFromShowRewardedAdv(Action<int> handler);
}