using System;

public interface IInGameUIViewController: IUIViewController, IInitable
{
    public Action OnResetButtonClick { get; set; }
    public Action OnEndScreenButtonClick { get; set; }
}
