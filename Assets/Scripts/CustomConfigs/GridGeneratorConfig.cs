using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = nameof(GridGeneratorConfig), menuName = "CustomConfigs/CoreConfigs/GridGeneratorConfig", order = 1)]
public class GridGeneratorConfig : ScriptableObject
{
    [SerializeField] private AssetReferenceGameObject _cellRef;
    [SerializeField] private int _greedSize;
    [SerializeField] private float _cellIndent;
    [SerializeField] private float _cellSize;

    public AssetReferenceGameObject CellRef => _cellRef;
    public int GridSize => _greedSize;
    public float CellIndent => _cellIndent;
    public float CellSize => _cellSize;
}
