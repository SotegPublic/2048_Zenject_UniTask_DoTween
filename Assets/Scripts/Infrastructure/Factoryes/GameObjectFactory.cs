using UnityEngine;
using Zenject;

public class GameObjectFactory : IGameObjectFactory
{
    private IInstantiator _instantiator;

    public GameObjectFactory(IInstantiator instantiator)
    {
        _instantiator = instantiator;
    }

    public GameObject Create(GameObject prefab, Vector2 position, Quaternion rotation, Transform parent)
    {
        return _instantiator.InstantiatePrefab(prefab, position, rotation, parent);
    }
}
