using System;
using UnityEngine;

public interface ISwipeInput
{
    public Action<Vector2> OnSwipeEnd { get; set; }
}
