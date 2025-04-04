using System;
using UnityEngine;

public class CalculateMoveState : BaseTurnState<IModelForMoveCalculations>
{
    private IGridHolder _gridHolder;

    private int _gridSize;
    private ICell[] _movingRow;

    public CalculateMoveState(IModelForMoveCalculations model, IGridHolder gameGridHolder) : base(model)
    {
        _gridHolder = gameGridHolder;
    }

    public void Init()
    {
        _gridSize = _gridHolder.Cells.GetLength(0);
        _movingRow = new ICell[_gridSize];
    }

    public override void ProcessedState()
    {
        _turnModel.IsNeedToMove = false;

        var moveTilesCount = 0;
        for (int i = 0; i < _gridSize; i++)
        {
            GetRow(i);
            CalculateTilesMove(ref moveTilesCount);
        }

        _turnModel.MoveTilesCount = moveTilesCount;
        _turnModel.OnStateEnd?.Invoke(this.GetType());
    }

    private void GetRow(int rowIndex)
    {
        switch (_turnModel.CurrentDir)
        {
            case DirectionType.Left:
                for (int i = _gridSize - 1; i >= 0; i--)
                {
                    var cellIndex = Mathf.Abs(i - (_gridSize - 1));
                    _movingRow[cellIndex] = _gridHolder.Cells[i, rowIndex];
                }
                break;
            case DirectionType.Right:
                for (int i = 0; i < _gridSize; i++)
                {
                    _movingRow[i] = _gridHolder.Cells[i, rowIndex];
                }
                break;
            case DirectionType.Up:
                for (int i = _gridSize - 1; i >= 0; i--)
                {
                    var cellIndex = Mathf.Abs(i - (_gridSize - 1));
                    _movingRow[cellIndex] = _gridHolder.Cells[rowIndex, i];
                }
                break;
            case DirectionType.Down:
                for (int i = 0; i < _gridSize; i++)
                {
                    _movingRow[i] = _gridHolder.Cells[rowIndex, i];
                }
                break;
            default:
            case DirectionType.None:
                throw new Exception("Unexepted direction in TilesController");
        }
    }

    private void CalculateTilesMove(ref int moveTilesCount)
    {
        for (int i = _gridSize - 1; i >= 0; i--)
        {
            if (!_movingRow[i].IsBusy)
                continue;
            if (_movingRow[i].GetNeighbour(_turnModel.CurrentDir) == null)
                continue;

            var pathLenth = 0;
            var tileValue = _movingRow[i].Tile.Value;
            var targetCell = GetTargetCell(_movingRow[i], _turnModel.CurrentDir, tileValue, ref pathLenth);

            if (_movingRow[i] != targetCell)
            {
                if (!_turnModel.IsNeedToMove)
                    _turnModel.IsNeedToMove = true;

                _turnModel.MoveModels[moveTilesCount] = new MoveModel
                {
                    Tile = _movingRow[i].Tile,
                    IsNeedToMerge = targetCell.IsBusy,
                    PathLenth = pathLenth,
                    TargetCell = targetCell
                };

                if (!targetCell.IsBusy)
                {
                    targetCell.BindNewTile(_movingRow[i].Tile);
                }
                else
                {
                    targetCell.Tile.UpdateValue(targetCell.Tile.Value * 2, true);
                }

                _movingRow[i].ClearCell();
                moveTilesCount++;
            }
        }
    }

    private ICell GetTargetCell(ICell cell, DirectionType dir, int tileValue, ref int lenth)
    {
        lenth++;
        var nextCell = cell.GetNeighbour(dir);

        if (nextCell == null)
            return cell;

        if (nextCell.IsBusy)
        {
            if (nextCell.Tile.Value == tileValue && !nextCell.Tile.IsMergedOnTurn)
            {
                lenth++;
                return nextCell;
            }
            else
                return cell;
        }
        else
        {
            return GetTargetCell(nextCell, dir, tileValue, ref lenth);
        }
    }
}
