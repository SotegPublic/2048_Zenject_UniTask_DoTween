using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class TileFactory : IFactory<Vector2, UniTask<TileView>>, IDisposable
{
    private IInstantiator _instantiator;
    
    private AssetReferenceGameObject _tileReference;
    private AsyncOperationHandle<GameObject> _tileHandle;

    public TileFactory(AssetReferenceGameObject tilePrefRef, IInstantiator zenjectInstantiator)
    {
        _instantiator = zenjectInstantiator;
        _tileReference = tilePrefRef;
    }

    public async UniTask<TileView> Create(Vector2 position)
    {
        if(!_tileHandle.IsValid())
        {
            await LoadPrefab();
        }

        if (_tileHandle.Status == AsyncOperationStatus.None)
        {
            await _tileHandle.Task;
        }

        if (_tileHandle.Status != AsyncOperationStatus.Succeeded)
            throw new Exception("Error on tile prefab load");

        var tileView = _instantiator.InstantiatePrefabForComponent<TileView>(_tileHandle.Result, position, Quaternion.identity, null);
        return tileView;
    }

    private async UniTask LoadPrefab()
    {
        _tileHandle = Addressables.LoadAssetAsync<GameObject>(_tileReference);
        await _tileHandle.Task;
    }

    public void Dispose()
    {
        Addressables.Release(_tileHandle);
    }
}
