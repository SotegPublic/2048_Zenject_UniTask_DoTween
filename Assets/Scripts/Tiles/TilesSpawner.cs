using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;

public class TilesSpawner : ITilesSpawner, IInitableTilesSpawner
{
    private ITileViewsPool _tilePool;
    private IGridHolder _gridHolder;
    private ITileViewUpdater _viewUpdater;

    private ICell[] _freeCells;
    private int _gridSize;

    public bool IsNoEmptyCell { get; private set; }

    public TilesSpawner(ITileViewsPool pool, IGridHolder holder, ITileViewUpdater tileViewUpdater)
    {
        _tilePool = pool;
        _gridHolder = holder;
        _viewUpdater = tileViewUpdater;
    }

    public void Init()
    {
        _gridSize = _gridHolder.Cells.GetLength(0);
        _freeCells = new ICell[_gridSize * _gridSize];
    }

    public async UniTask SpawnTile()
    {
        if (IsNoEmptyCell)
            IsNoEmptyCell = false;

        var value = Random.Range(0, 101) < 90 ? 2 : 4;
        var targetCell = GetFreeCell();

        if (targetCell == null)
            return;

        await CreateNewTile(value, targetCell);
    }

    public async UniTask SpawnTile(int value, int cellID)
    {
        var targetCell = GetCellByID(cellID);

        if (targetCell == null)
            throw new Exception("Error in save data. Wrong cellID");

        await CreateNewTile(value, targetCell);
    }

    private async UniTask CreateNewTile(int value, ICell targetCell)
    {
        var tileView = await _tilePool.GetTile(targetCell.Position);

        tileView.transform.DOScale(0.4f, 0.1f)
            .SetEase(Ease.OutBack)
            .SetLink(tileView.gameObject).ToUniTask().Forget();

        var tile = CreateTile(tileView, value);
        targetCell.BindNewTile(tile);
    }

    public ICell GetFreeCell()
    {
        var freeCellsCount = 0;

        for (int i = 0; i < _gridSize; i++)
        {
            var cell = _gridHolder.Cells[i, 0];

            while(cell != null)
            {
                if(!cell.IsBusy)
                {
                    _freeCells[freeCellsCount] = cell;
                    freeCellsCount++;
                }
                cell = cell.GetNeighbour(DirectionType.Down);
            }
        }

        if(freeCellsCount == 1)
            IsNoEmptyCell = true;

        return freeCellsCount > 0 ? _freeCells[Random.Range(0, freeCellsCount)] : null;
    }

    public ICell GetCellByID(int id)
    {
        for (int i = 0; i < _gridSize; i++)
        {
            var cell = _gridHolder.Cells[i, 0];

            while (cell != null)
            {
                if (cell.CellID == id)
                {
                    return cell;
                }
                cell = cell.GetNeighbour(DirectionType.Down);
            }
        }

        return null;
    }

    private Tile CreateTile(TileView tileView, int value)
    {
        var tile = new Tile(tileView);
        tile.UpdateValue(value, false);
        _viewUpdater.UpdateView(tileView, value);

        return tile;
    }
}