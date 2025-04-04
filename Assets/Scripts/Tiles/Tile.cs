using System;
using UnityEngine;

public class Tile
{
    private TileView _tileView;
    private Transform _tileTransform;
    private int _value;
    private bool _isMergedOnTurn;

    public Tile(TileView view)
    {
        _tileView = view;
        _tileTransform = _tileView.transform;
    }

    public int Value => _value;
    public Transform TileTransform => _tileTransform;
    public TileView TileView => _tileView;
    public bool IsMergedOnTurn => _isMergedOnTurn;

    public void UpdateValue(int newValue, bool mergeUpdate)
    {
        _value = newValue;
        _isMergedOnTurn = mergeUpdate;
    }

    public void ResetMergedFlag() => _isMergedOnTurn = false;

    public void Clear()
    {
        _value = 0;
        _tileTransform = null;
        _tileView = null;
        _isMergedOnTurn = false;
    }
}
