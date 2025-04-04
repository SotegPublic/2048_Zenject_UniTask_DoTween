using System;
using UnityEngine;

public class Cell: ICell, IEquatable<Cell>
{
    private Vector2 _position;
    private bool _isBusy;
    private Tile _tile;
    private int _id;

    private ICell _upCell;
    private ICell _downCell;
    private ICell _leftCell;
    private ICell _rightCell;
    
    public Tile Tile => _tile;
    public bool IsBusy => _isBusy;
    public Vector2 Position => _position;
    public int CellID => _id;

    public Cell(Vector2 cellPosition, int cellID)
    {
        _position = cellPosition;
        _id = cellID;
    }

    public void BindNeighbours(Cell up, Cell down, Cell left, Cell right)
    {
        _upCell = up;
        _downCell = down;
        _leftCell = left;
        _rightCell = right;
    }

    public ICell GetNeighbour(DirectionType dir)
    {
        switch (dir)
        {
            case DirectionType.Left:
                return _leftCell;
            case DirectionType.Right:
                return _rightCell;
            case DirectionType.Up:
                return _upCell;
            case DirectionType.Down:
                return _downCell;
            default:
            case DirectionType.None:
                throw new Exception("Unexcepted direction");
        }
    }
    public void BindNewTile(Tile newTile)
    {
        _tile = newTile;
        _isBusy = true;
    }
    public void ClearCell()
    {
        _tile = null;
        _isBusy = false;
    }

    public bool Equals(Cell other)
    {
        return _id == other.CellID;
    }
}
