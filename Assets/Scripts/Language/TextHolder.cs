using TMPro;
using UnityEngine;

public class TextHolder : MonoBehaviour, ITextHolder
{
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private TextModel[] _textModels;

    public void SetText(LanguageType languageType)
    {
        for(int i = 0; i < _textModels.Length; i++)
        {
            if (_textModels[i].LanguageType == languageType)
            {
                _textField.text = _textModels[i].Text;
            }
        }
    }
}
