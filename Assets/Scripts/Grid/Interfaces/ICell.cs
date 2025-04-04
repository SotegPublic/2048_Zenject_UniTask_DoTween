using UnityEngine;

public interface ICell
{
    public Tile Tile { get; }
    public bool IsBusy { get; }
    public Vector2 Position { get; }
    public int CellID { get; }
    public void BindNewTile(Tile newTile);
    public void ClearCell();
    public ICell GetNeighbour(DirectionType dir);
}
