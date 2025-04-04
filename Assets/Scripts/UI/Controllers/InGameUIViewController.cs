using System;
using Zenject;

public class InGameUIViewController: IInGameUIViewController, IUIViewController, IInitable, IEndScreenController, IResettingEndScreenController, IDisposable
{
    private InGameUIView _view;
    private IPlayerModel _playerModel;
    private ISoundManager _soundManager;
    private ISaveSystem _saveSystem;

    public Action OnResetButtonClick { get; set; }
    public Action OnEndScreenButtonClick { get; set; }

    [Inject]
    public InGameUIViewController(InGameUIView uiView, IPlayerModel model, ISoundManager gameSoundManager, ISaveSystem gameSaveSystem)
    {
        _view = uiView;
        _playerModel = model;

        _view.RestartButton.onClick.AddListener(ResetGame);
        _view.SoundButton.onClick.AddListener(SoundSwitch);
        _view.EndScreenButton.onClick.AddListener(EndScreenButtonClick);
        _soundManager = gameSoundManager;
        _saveSystem = gameSaveSystem;

        _playerModel.Score.Subscribe(UpdateScoreText);
        _playerModel.Record.Subscribe(UpdateRecordText);
    }
    public void Init()
    {
        _view.RestartButton.interactable = true;
        _view.SoundButton.image.sprite = _playerModel.IsSoundOff ? _view.SoundOffSprite : _view.SoundOnSprite;
        _view.ScoreText.text = _playerModel.Score.Value.ToString();
        _view.RecordText.text = _playerModel.Record.Value.ToString();
    }

    private void SoundSwitch()
    {
        _playerModel.IsSoundOff = !_playerModel.IsSoundOff;
        _view.SoundButton.image.sprite = _playerModel.IsSoundOff ? _view.SoundOffSprite : _view.SoundOnSprite;
        _soundManager.SwitchSound(_playerModel.IsSoundOff);
        _saveSystem.Save();
    }

    private void UpdateScoreText(int score)
    {
        _view.ScoreText.text = score.ToString();
    }

    private void UpdateRecordText(int record)
    {
        _view.RecordText.text = record.ToString();
    }

    private void ResetGame()
    {
        OnResetButtonClick?.Invoke();
        _view.RestartButton.interactable = false;
    }

    private void EndScreenButtonClick()
    {
        OnEndScreenButtonClick?.Invoke();
    }

    public void ShowEndScreen(bool isWin)
    {
        _view.WinText.gameObject.SetActive(isWin);
        _view.LoseText.gameObject.SetActive(!isWin);
        _view.WinButtonText.gameObject.SetActive(isWin);
        _view.LoseButtonText.gameObject.SetActive(!isWin);

        _view.EndScreenImage.color = isWin ? _view.WinColor : _view.LoseColor;

        _view.EndScreenCanvasGroup.Show();
    }

    public void HideEndScreen()
    {
        _view.EndScreenCanvasGroup.Hide();
    }

    public void ShowUI()
    {
        _view.CanvasGroup.Show();
    }

    public void HideUI()
    {
        _view.CanvasGroup.Hide();
    }

    public void Dispose()
    {
        _view.RestartButton.onClick.RemoveListener(ResetGame);
        _view.SoundButton.onClick.RemoveListener(SoundSwitch);

        _playerModel.Score.Unsubscribe(UpdateScoreText);
        _playerModel.Record.Unsubscribe(UpdateRecordText);
    }
}
