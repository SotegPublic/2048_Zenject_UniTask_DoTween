using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameBootstrapper : MonoBehaviour
{
    private IGameStateMachine _gameStateMachine;

    [Inject]
    public void Construct(IGameStateMachine stateMachine)
    {
        _gameStateMachine = stateMachine;
    }

    private void Start()
    {
        _gameStateMachine.StartGame();
    }
}
