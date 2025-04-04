using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = nameof(TilesConfig), menuName = "CustomConfigs/CoreConfigs/TilesConfig", order = 0)]
public class TilesConfig : ScriptableObject
{
    [SerializeField] private AssetReferenceGameObject _tileRef;
    [SerializeField] private float _tileMoveSpeed;

    public AssetReferenceGameObject TileRef => _tileRef;
    public float TileMoveSpeed => _tileMoveSpeed;
}
