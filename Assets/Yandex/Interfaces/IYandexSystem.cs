using System;

public interface IYandexSystem
{
    public void YandexDebug(string text);
    public bool IsYandexInit();
    public void CheckAuthentificated();
    public void CallAuthentification();
    public string GetLang();
    public string GetDevice();
    public string GetName();
    public string GetUID();
    public DateTime GetTime();
    public void SetLeaderBoardValue(string leaderboardID, int value);
    //public void UpdateLeaderBoard(string lbName, int topQuantity, bool isUserIncluded, int aroundQuantity);
    public void SendGameReady();
    public void SendRateRequest();
    public void ShowAdvertising();
    public void ShowRewardedAdvertising(int rewardID);
}
