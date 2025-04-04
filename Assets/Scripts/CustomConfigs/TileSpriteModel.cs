using System;
using UnityEngine;

[Serializable]
public class TileSpriteModel
{
    [SerializeField] private int _value;
    [SerializeField] private Sprite _sprite;

    public int Value => _value;
    public Sprite Sprite => _sprite;
}