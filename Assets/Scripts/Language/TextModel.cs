using System;
using UnityEngine;

[Serializable]
public class TextModel
{
    [SerializeField] private LanguageType _languageType;
    [SerializeField, TextArea] private string _text;

    public LanguageType LanguageType => _languageType;
    public string Text => _text;
}
