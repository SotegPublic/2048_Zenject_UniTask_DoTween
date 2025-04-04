using System;

public interface IEndScreenController
{
    public Action OnEndScreenButtonClick { get; set; }
    public void ShowEndScreen(bool isWin);
    public void HideEndScreen();
}
