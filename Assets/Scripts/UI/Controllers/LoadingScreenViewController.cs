using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenViewController : IGetAuthRequestResultNotification, IProgressBarViewController, IUIViewController, IDisposable, ILoadingScreenViewController
{
    private LoadingScreenView _view;
    private IYandexSystem _yandex;

    public Action OnStartGameButtonClick { get; set; }
    
    public LoadingScreenViewController(LoadingScreenView screenView, IYandexSystem yandexSystem)
    {
        _view = screenView;
        _yandex = yandexSystem;

        _view.StartGameButton.onClick.AddListener(StartGame);
        _view.AuthButton.onClick.AddListener(SendAuthRequest);
        _view.CancelAuthButton.onClick.AddListener(StartGame);

        _view.LoadProgressImage.fillAmount = 0;
    }

    private void SendAuthRequest()
    {
        _yandex.CallAuthentification();
    }

    private void StartGame()
    {
        OnStartGameButtonClick?.Invoke();
    }

    private void RedrawButtons(bool isAuth)
    {
        _view.StartGameButton.gameObject.SetActive(isAuth);
        _view.CancelAuthButton.gameObject.SetActive(!isAuth);
        _view.AuthButton.gameObject.SetActive(!isAuth);
    }

    public void HideAuthText()
    {
        _view.AuthText.gameObject.SetActive(false);
    }

    public void GetAuthRequestResult(bool isAuth)
    {
        if(isAuth)
        {
            HideAuthText();
        }

        RedrawButtons(isAuth);
    }

    public void ShowButtonsBlock(bool isAuth)
    {
        RedrawButtons(isAuth);

        _view.ButtonsCanvasGroup.Show();
    }

    public void HideButtonsBlock()
    {
        _view.ButtonsCanvasGroup.Hide();
    }

    public void ShowProgressBar()
    {
        _view.ProgressBarCanvasGroup.Show();
    }

    public void HideUI()
    {
        _view.PanelCanvasGroup.Hide();
    }

    public void ShowUI()
    {
        _view.PanelCanvasGroup.Show();
    }

    public void FillProgressBar(float value)
    {
        _view.LoadProgressImage.fillAmount += value;
    }

    public void Dispose()
    {
        _view.StartGameButton.onClick.RemoveListener(StartGame);
        _view.AuthButton.onClick.RemoveListener(SendAuthRequest);
        _view.CancelAuthButton.onClick.RemoveListener(StartGame);
    }
}
