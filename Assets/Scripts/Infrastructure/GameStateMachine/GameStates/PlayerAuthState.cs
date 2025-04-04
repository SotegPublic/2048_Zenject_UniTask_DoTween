using System;
using System.Threading;
using Zenject;

public class PlayerAuthState : BaseState, IGetAuthRequestResultNotification, IGetAuthCheckResultNotification
{
    private IYandexSystem _yandex;
    private IChangeblePlayerInfoHolder _playerInfoHolder;
    private ILoadingScreenViewController _loadingScreenViewController;
    private ILanguageController _tranclateController;

    [Inject]
    public PlayerAuthState(IYandexSystem yandexSystem, IChangeblePlayerInfoHolder infoHolder, ILoadingScreenViewController viewController, ILanguageController languageController)
    {
        _yandex = yandexSystem;
        _playerInfoHolder = infoHolder;
        _loadingScreenViewController = viewController;
        _tranclateController = languageController;

        _loadingScreenViewController.OnStartGameButtonClick += StartGame; 
    }

    private void StartGame()
    {
        OnStateEnd?.Invoke(this.GetType());
    }

    public override void EnterState()
    {
        var lang = _yandex.GetLang();
        _tranclateController.SetLanguage(lang);
        
        _loadingScreenViewController.ShowUI();
        _yandex.CheckAuthentificated();
    }

    public override void ExitState()
    {
        _loadingScreenViewController.HideButtonsBlock();
        _loadingScreenViewController.ShowProgressBar();
    }

    public void GetAuthCheckResult(bool isAuth)
    {
        if(isAuth)
        {
            SetPlayerInfo(isAuth);
            _loadingScreenViewController.HideAuthText();
        }

        _loadingScreenViewController.ShowButtonsBlock(isAuth);
        _yandex.SendGameReady();
    }

    public void GetAuthRequestResult(bool isAuth)
    {
        SetPlayerInfo(isAuth);
    }

    private void SetPlayerInfo(bool isAuth)
    {
        _playerInfoHolder.SetIsAuthenticated(isAuth);

        if (isAuth)
        {
            _playerInfoHolder.SetPlayerName(_yandex.GetName());
            _playerInfoHolder.SetPlayerUID(_yandex.GetUID());
        }
    }
}
