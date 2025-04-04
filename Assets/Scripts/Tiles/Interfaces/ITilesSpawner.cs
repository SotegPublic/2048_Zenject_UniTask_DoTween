using Cysharp.Threading.Tasks;

public interface ITilesSpawner
{
    public UniTask SpawnTile();
    public UniTask SpawnTile(int value, int cellID);
    public bool IsNoEmptyCell { get; }
}
