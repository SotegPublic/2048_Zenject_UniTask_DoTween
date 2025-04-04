#if UNITY_EDITOR
using System;
public class YandexSystemInEditorMediator: IYandexSystemInEditorEventsPublisher, IYandexSystemInEditorSubscriber
{
    private event Action<bool> _onGetAuthCheckResult;
    private event Action<bool> _onGetAuthRequestResult;
    private event Action _onAdvClosed;
    private event Action<int> _onRewardedAdvClosed;
    private event Action<int> _onGetAdvReward;

    //Паблишинг
    public void PublishAuthCheckResult(bool isSuccess) => _onGetAuthCheckResult?.Invoke(isSuccess);
    public void PublishAuthRequestResult(bool isSuccess) => _onGetAuthRequestResult?.Invoke(isSuccess);
    public void PublishAdvClosed() => _onAdvClosed?.Invoke();
    public void PublishRewardedAdvClosed(int rewardId) => _onRewardedAdvClosed?.Invoke(rewardId);
    public void PublishAdvReward(int rewardId) => _onGetAdvReward?.Invoke(rewardId);

    // Подписка/отписка
    public void SubscribeToAuthCheck(Action<bool> handler) => _onGetAuthCheckResult += handler;
    public void UnsubscribeFromAuthCheck(Action<bool> handler) => _onGetAuthCheckResult -= handler;

    public void SubscribeToAuthRequest(Action<bool> handler) => _onGetAuthRequestResult += handler;
    public void UnsubscribeFromAuthRequest(Action<bool> handler) => _onGetAuthRequestResult -= handler;
    public void SubscribeToAdvClosed(Action handler) => _onAdvClosed += handler;
    public void UnsubscribeFromAdvClosed(Action handler) => _onAdvClosed -= handler;
    public void SubscribeToRewardedAdvClosed(Action<int> handler) => _onRewardedAdvClosed += handler;
    public void UnsubscribeFromRewardedAdvClosed(Action<int> handler) => _onRewardedAdvClosed -= handler;

    public void SubscribeToAdvReward(Action<int> handler) => _onGetAdvReward += handler;
    public void UnsubscribeFromAdvReward(Action<int> handler) => _onGetAdvReward -= handler;
}


public interface IYandexSystemInEditorEventsPublisher
{
    void PublishAuthCheckResult(bool isSuccess);
    void PublishAuthRequestResult(bool isSuccess);
    void PublishAdvClosed();
    void PublishRewardedAdvClosed(int rewardId);
    void PublishAdvReward(int rewardId);
}

public interface IYandexSystemInEditorSubscriber
{
    // Авторизация
    void SubscribeToAuthCheck(Action<bool> handler);
    void UnsubscribeFromAuthCheck(Action<bool> handler);

    void SubscribeToAuthRequest(Action<bool> handler);
    void UnsubscribeFromAuthRequest(Action<bool> handler);

    // Реклама
    void SubscribeToAdvClosed(Action handler);
    void UnsubscribeFromAdvClosed(Action handler);

    // Rewarded реклама
    void SubscribeToRewardedAdvClosed(Action<int> handler);
    void UnsubscribeFromRewardedAdvClosed(Action<int> handler);

    // Награды
    void SubscribeToAdvReward(Action<int> handler);
    void UnsubscribeFromAdvReward(Action<int> handler);
}
#endif