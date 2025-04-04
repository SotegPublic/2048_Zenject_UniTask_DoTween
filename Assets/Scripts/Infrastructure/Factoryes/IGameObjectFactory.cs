using UnityEngine;
using Zenject;

public interface IGameObjectFactory : IFactory<GameObject, Vector2, Quaternion, Transform, GameObject>
{
}
