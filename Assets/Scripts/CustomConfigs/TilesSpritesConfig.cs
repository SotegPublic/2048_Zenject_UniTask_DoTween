using UnityEngine;

[CreateAssetMenu(fileName = nameof(TilesSpritesConfig), menuName = "CustomConfigs/CoreConfigs/TilesSpritesConfig", order = 1)]
public class TilesSpritesConfig : ScriptableObject
{
    [SerializeField] private TileSpriteModel[] _models;

    public TileSpriteModel[] TileSpritesModels => _models;
}
