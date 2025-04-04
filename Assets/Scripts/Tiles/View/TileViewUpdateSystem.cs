using System;
using UnityEngine;

public class TileViewUpdateSystem : ITileViewUpdater
{
    private TilesSpritesConfig _config;

    public TileViewUpdateSystem(TilesSpritesConfig tilesSpritesConfig)
    {
        _config = tilesSpritesConfig;
    }

    public void UpdateView(TileView view, int newValue)
    {
        var sprite = GetSprite(newValue);
        view.SpriteRenderer.sprite = sprite;
    }

    private Sprite GetSprite(int newValue)
    {
        for(int i = 0; i < _config.TileSpritesModels.Length; i++)
        {
            if (_config.TileSpritesModels[i].Value == newValue)
                return _config.TileSpritesModels[i].Sprite;
        }

        throw new Exception("Unknown value in TileViewUpdateSystem");
    }
}
