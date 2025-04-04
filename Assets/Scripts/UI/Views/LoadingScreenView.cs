using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenView : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _authButton;
    [SerializeField] private Button _cancelAuthButton;
    [SerializeField] private Image _loadProgressImage;
    [SerializeField] private CanvasGroup _panelCanvasGroup;
    [SerializeField] private CanvasGroup _buttonsCanvasGroup;
    [SerializeField] private CanvasGroup _progressBarCanvasGroup;
    [SerializeField] private TMP_Text _authText;

    public Button StartGameButton => _startGameButton;
    public Button AuthButton => _authButton;
    public Button CancelAuthButton => _cancelAuthButton;
    public Image LoadProgressImage=> _loadProgressImage;
    public CanvasGroup PanelCanvasGroup => _panelCanvasGroup;
    public CanvasGroup ButtonsCanvasGroup => _buttonsCanvasGroup;
    public CanvasGroup ProgressBarCanvasGroup => _progressBarCanvasGroup;
    public TMP_Text AuthText => _authText;
}
