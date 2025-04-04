using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LoadPlayerState : BaseState, IGetLoadExternNotification
{
    private ILoadDataProvider _loadDataProvider;
    private IPlayerInfoHolder _playerInfoHolder;
    private ILoadableModel _playerSaveModel;
    private ISaveSystem _saveSystem;
    private IProgressBarViewController _progressBarViewController;

    [Inject]
    public LoadPlayerState(ILoadDataProvider loadProvider, IPlayerInfoHolder infoHolder, ILoadableModel saveModel, ISaveSystem saveGameSystem,
        IProgressBarViewController uiViewController)
    {
        _loadDataProvider = loadProvider;
        _playerInfoHolder = infoHolder;
        _playerSaveModel = saveModel;
        _saveSystem = saveGameSystem;
        _progressBarViewController = uiViewController;
    }

    public override void EnterState()
    {
        _progressBarViewController.FillProgressBar(0.2f);
        
        if(_playerInfoHolder.IsAuthenticated)
        {
            _loadDataProvider.LoadFromCloud();
        }
        else
        {
            if(_loadDataProvider.TryLoadLocal(out var loadJson))
            {
                LoadData(loadJson);
            }
            else
            {
                LoadInitialSave();
                OnStateEnd?.Invoke(this.GetType());
            }
        }
    }

    public override void ExitState()
    {
        _progressBarViewController.FillProgressBar(0.3f);
    }

    public void GetLoadExtern(string loadJson)
    {
        LoadData(loadJson);
    }

    public void LoadData(string loadJson)
    {
        if(string.IsNullOrEmpty(loadJson))
        {
            LoadInitialSave();
        }
        else
        {
            PlayerSaveModel data = JsonUtility.FromJson<PlayerSaveModel>(loadJson);

            if (data.IsModelInited)
            {
                _playerSaveModel.LoadModel(data);
            }
            else
            {
                LoadInitialSave();
            }
        }

        OnStateEnd?.Invoke(this.GetType());
    }

    private void LoadInitialSave()
    {
        _playerSaveModel.LoadModel(new PlayerSaveModel
        {
            IsModelInited = true,
            Score = 0,
            IsSoundOff = false,
            TilesOnField = new List<TileSaveModel>(16)
        });

        _saveSystem.Save();
    }
}
