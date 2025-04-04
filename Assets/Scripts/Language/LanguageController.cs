using System.Collections.Generic;

public class LanguageController : ILanguageController
{
    private TextHolder[] _textHolders;

    public LanguageController(TextHolder[] uitextHolders)
    {
        _textHolders = uitextHolders;
    }

    public void SetLanguage(string language)
    {
        var languageType = GetLanguageType(language);

        for(int i = 0; i < _textHolders.Length; i++)
        {
            _textHolders[i].SetText(languageType);
        }
    }

    private LanguageType GetLanguageType(string language)
    {
        switch (language)
        {
            case "ru":
                return LanguageType.Ru;
            case "tr":
                return LanguageType.Tr;
            case "en":
            default:
                return LanguageType.En;
        }
    }
}
