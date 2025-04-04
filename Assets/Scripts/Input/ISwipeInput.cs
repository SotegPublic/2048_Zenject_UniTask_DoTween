using System;
using UnityEngine;

public interface ISwipeInput
{
    public Action<Vector2> OnSwipe { get; set; }
    public Action OnSwipeEnd { get; set; }
}
