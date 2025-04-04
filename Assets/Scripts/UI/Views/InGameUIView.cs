using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InGameUIView : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Sprite _soundOffSprite;
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _recordText;

    [Header("End Screen")]
    [SerializeField] private CanvasGroup _endScreenCanvasGroup;
    [SerializeField] private TMP_Text _winText;
    [SerializeField] private TMP_Text _loseText;
    [SerializeField] private Button _endScreenButton;
    [SerializeField] private TMP_Text _winButtonText;
    [SerializeField] private TMP_Text _loseButtonText;
    [SerializeField] private Image _endScreenImage;
    [SerializeField] private Color _winColor;
    [SerializeField] private Color _loseColor;

    public Button RestartButton => _restartButton;
    public Button SoundButton => _soundButton;
    public CanvasGroup CanvasGroup => _canvasGroup;
    public Sprite SoundOffSprite => _soundOffSprite;
    public Sprite SoundOnSprite => _soundOnSprite;
    public TMP_Text ScoreText => _scoreText;
    public TMP_Text RecordText => _recordText;

    public CanvasGroup EndScreenCanvasGroup => _endScreenCanvasGroup;
    public TMP_Text WinText => _winText;
    public TMP_Text LoseText => _loseText;
    public Button EndScreenButton => _endScreenButton;
    public TMP_Text WinButtonText => _winButtonText;
    public TMP_Text LoseButtonText => _loseButtonText;
    public Image EndScreenImage => _endScreenImage;
    public Color WinColor => _winColor;
    public Color LoseColor => _loseColor;
}
