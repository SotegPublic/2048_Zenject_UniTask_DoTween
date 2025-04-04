using System;

public interface ILoadingScreenViewController : IUIViewController
{
    public Action OnStartGameButtonClick { get; set; }
    public void HideAuthText();
    public void ShowButtonsBlock(bool isAuth);
    public void HideButtonsBlock();
    public void ShowProgressBar();
}
