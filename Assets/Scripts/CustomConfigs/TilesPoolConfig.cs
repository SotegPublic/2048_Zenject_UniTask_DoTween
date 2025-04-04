using UnityEngine;

[CreateAssetMenu(fileName = nameof(TilesPoolConfig), menuName = "CustomConfigs/CoreConfigs/TilesPoolConfig", order = 2)]
public class TilesPoolConfig : ScriptableObject
{
    [SerializeField] private int _warmupTilesCount;
    [SerializeField] private string _hidenLayerName;
    [SerializeField] private string _shownLayerName;

    public int WarmupTilesCount => _warmupTilesCount;
    public string HidenLayerName => _hidenLayerName;
    public string ShownLayerName => _shownLayerName;
}