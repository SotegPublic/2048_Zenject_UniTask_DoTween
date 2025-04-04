using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ITileViewsPool
{
    public UniTask<TileView> GetTile(Vector2 position);
    public void ReliseView(TileView tile);
}
