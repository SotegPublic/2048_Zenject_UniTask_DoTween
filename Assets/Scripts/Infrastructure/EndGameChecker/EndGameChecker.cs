using System;

public class EndGameChecker: IEndGameChecker
{
    private IGridHolder _gridHolder;

    public Action OnGameOver { get; set; }

    public EndGameChecker(IGridHolder gridHolder)
    {
        _gridHolder = gridHolder;
    }

    public void GameOverCheck()
    {
        for (int i = 0; i < _gridHolder.Cells.GetLength(0); i++)
        {
            for (int j = 0; j < _gridHolder.Cells.GetLength(1); j++)
            {
                var cell = _gridHolder.Cells[i, j];

                if (!cell.IsBusy || HasEmptyOrMatchingNeighbor(cell, DirectionType.Right) ||
                        HasEmptyOrMatchingNeighbor(cell, DirectionType.Down))
                {
                    return;
                }
            }
        }

        OnGameOver?.Invoke();
    }

    private bool HasEmptyOrMatchingNeighbor(ICell cell, DirectionType direction)
    {
        var neighbor = cell.GetNeighbour(direction);
        if (neighbor == null) return false;

        return !neighbor.IsBusy || (neighbor.Tile.Value == cell.Tile.Value);
    }
}
