using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : IDisposable, ISwipeInput, ITickable
{
    public Action<Vector2> OnSwipe { get; set; }
    public Action OnSwipeEnd { get; set; }
    
    private PlayerInput _playerInput;
    private bool _isSwipe;
    private Vector2 _startPosition;

    [Inject]
    public void Construct()
    {
        _playerInput = new PlayerInput();
        _playerInput.Enable();

        _playerInput.Player.Touch.performed += OnTouchStart;
        _playerInput.Player.Touch.canceled += OnTouchEnd;
    }

    private void OnTouchStart(CallbackContext context)
    {
        _startPosition = _playerInput.Player.Swipe.ReadValue<Vector2>();
        _isSwipe = true;
    }

    private void OnTouchEnd(CallbackContext context)
    {
        _isSwipe = false;
        OnSwipeEnd?.Invoke();
    }

    public void LocalUpdate()
    {
        if (!_isSwipe)
            return;

        var delta = _startPosition - _playerInput.Player.Swipe.ReadValue<Vector2>();

        OnSwipe?.Invoke(delta);
    }

    public void Dispose()
    {
        _playerInput.Disable();
    }

    public void Tick()
    {
        LocalUpdate();
    }
}
