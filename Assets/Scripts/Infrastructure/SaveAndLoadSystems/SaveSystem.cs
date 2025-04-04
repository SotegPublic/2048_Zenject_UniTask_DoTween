using System;
using UnityEngine;
using Zenject;

public class SaveSystem : ISaveSystem
{
    private IGridHolder _gridHolder;
    private ISaveProvider _saveProvider;
    private IYandexSystem _yandex;
    private IPlayerModel _playerModel;
    private IPlayerInfoHolder _playerInfoHolder;

    private DateTime _lastSaveTime;

    [Inject]
    public SaveSystem(IGridHolder gameGridHolder, ISaveProvider saveGameProvider, IPlayerModel saveModel, IPlayerInfoHolder infoHolder, IYandexSystem yandexSystem)
    {
        _gridHolder = gameGridHolder;
        _saveProvider = saveGameProvider;
        _playerModel = saveModel;
        _playerInfoHolder = infoHolder;
        _yandex = yandexSystem;
    }

    public void Save()
    {
        var currentTime = _yandex.GetTime();
        var dif = currentTime - _lastSaveTime;

        if (dif.TotalSeconds < 3)
            return;

        FillTilesModels();

        var saveModel = new PlayerSaveModel
        {
            Score = _playerModel.Score.Value,
            Record = _playerModel.Record.Value,
            IsSoundOff= _playerModel.IsSoundOff,
            IsModelInited = _playerModel.IsModelInited,
            TilesOnField = _playerModel.TilesOnField
        };

        var saveData = JsonUtility.ToJson(saveModel);

        if(_playerInfoHolder.IsAuthenticated)
        {
            _saveProvider.SaveGameExtern(saveData);
        }
        else
        {
            _saveProvider.SaveGameLocal(saveData);
        }

        _lastSaveTime = currentTime;
    }

    private void FillTilesModels()
    {
        if (_gridHolder.Cells == null)
            return;

        _playerModel.TilesOnField.Clear();

        for(int i = 0; i < _gridHolder.Cells.GetLength(0); i++)
        {
            var cell = _gridHolder.Cells[i, 0];

            while (cell != null)
            {
                if (cell.IsBusy)
                {
                    _playerModel.TilesOnField.Add(new TileSaveModel
                    {
                        CellID = cell.CellID,
                        Value = cell.Tile.Value
                    });
                }

                cell = cell.GetNeighbour(DirectionType.Down);
            }
        }
    }
}
