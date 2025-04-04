using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class GridGenerator: IGridHolder, IAwaitableOnInit
{
    private GridGeneratorConfig _config;
    private IGameObjectFactory _goFactory;

    private GameObject _cellPref;
    private Cell[,] _cells;

    public ICell[,] Cells => _cells;

    [Inject]
    public GridGenerator(GridGeneratorConfig generatorConfig, IGameObjectFactory gameObjectFactory)
    {
        _config = generatorConfig;
        _goFactory = gameObjectFactory;
    }

    public async UniTask Init()
    {
        var cellHandle = Addressables.LoadAssetAsync<GameObject>(_config.CellRef);
        _cellPref = await cellHandle.Task;
        _cells = new Cell[_config.GridSize, _config.GridSize];

        if (cellHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GenerateGrid();
            BindNeighbours();
        }
        else
            throw new Exception("Error on cell prefab load");

    }

    private void GenerateGrid()
    {
        var totalWidth = _config.GridSize * _config.CellSize + (_config.GridSize - 1) * _config.CellIndent;
        var startX = -totalWidth * 0.5f + _config.CellSize * 0.5f;
        var startY = totalWidth * 0.5f - _config.CellSize * 0.5f;

        var cellNumber = 0;

        for (int x = 0; x < _config.GridSize; x++)
        {
            for (int y = 0; y < _config.GridSize; y++)
            {
                var position = new Vector2(startX + x * (_config.CellSize + _config.CellIndent), startY - y * (_config.CellSize + _config.CellIndent));
                
                var cell = new Cell(position, cellNumber);
                _cells[x, y] = cell;
                SpawnCell(position);
                cellNumber++;
            }
        }
    }

    private void BindNeighbours()
    {
        for (int x = 0; x < _config.GridSize; x++)
        {
            for (int y = 0; y < _config.GridSize; y++)
            {
                var upCell = y == 0 ? null : _cells[x, y - 1];
                var downCell = y == _config.GridSize - 1 ? null : _cells[x, y + 1];
                var leftCell = x == 0 ? null : _cells[x - 1, y];
                var rightCell = x == _config.GridSize - 1 ? null : _cells[x + 1, y];

                _cells[x,y].BindNeighbours(upCell, downCell, leftCell, rightCell);
            }
        }
    }

    private void SpawnCell(Vector2 position)
    {
        _goFactory.Create(_cellPref, position, Quaternion.identity, null);
    }
}
