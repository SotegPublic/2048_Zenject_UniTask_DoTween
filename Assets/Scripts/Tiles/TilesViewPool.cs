using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TilesViewPool: ITileViewsPool, IAwaitableOnInit
{
    private IFactory<Vector2, UniTask<TileView>> _tileFactory;

    private int _gridSize;
    private int _hidenLayer;
    private int _shownLayer;
    private int _warmUpCount;
    private Vector2 _poolPosition;

    private List<TileView> _tiles;
    private List<UniTask<TileView>> _tasksList;

    public TilesViewPool(IFactory<Vector2, UniTask<TileView>> factory, TilesPoolConfig tilesPoolConfig, int size, Vector3 position)
    {
        _tileFactory = factory;
        _gridSize = size;
        _warmUpCount = tilesPoolConfig.WarmupTilesCount;
        _poolPosition = position;
        _hidenLayer = LayerMask.NameToLayer(tilesPoolConfig.HidenLayerName);
        _shownLayer = LayerMask.NameToLayer(tilesPoolConfig.ShownLayerName);
    }

    public async UniTask Init()
    {
        _tiles = new List<TileView>(_gridSize);
        _tasksList = new List<UniTask<TileView>>(_warmUpCount);

        for(int i = 0; i < _warmUpCount; i++)
        {
            _tasksList.Add(_tileFactory.Create(_poolPosition));
        }

        await UniTask.WhenAll(_tasksList);
        _tasksList.Clear();
    }

    public async UniTask<TileView> GetTile(Vector2 position)
    {
        TileView view = null;

        if (_tiles.Count > 0)
        {
            view = _tiles[_tiles.Count - 1];
            _tiles.RemoveAt(_tiles.Count - 1);
        }
        else
        {
            view = await _tileFactory.Create(_poolPosition);
        }

        view.gameObject.transform.position = position;
        view.gameObject.layer = _shownLayer;
        view.gameObject.transform.localScale = Vector3.zero;

        return view;
    }

    public void ReliseView(TileView view)
    {
        view.gameObject.layer = _hidenLayer;
        view.gameObject.transform.position = _poolPosition;
        view.SpriteRenderer.sprite = null;
        view.MergeParticle.Stop();
        _tiles.Add(view);
    }
}
