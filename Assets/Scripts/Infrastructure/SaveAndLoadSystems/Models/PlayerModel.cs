using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class PlayerModel: IPlayerModel, ILoadableModel
{
    public Score Score { get; set; }
    public Record Record { get; set; }
    public bool IsSoundOff { get; set; }
    public List<TileSaveModel> TilesOnField { get; set; } = new List<TileSaveModel>(16);
    public bool IsModelInited { get; set; }

    public PlayerModel()
    {
        Score = new Score();
        Record= new Record();
    }
    
    void ILoadableModel.LoadModel(PlayerSaveModel model)
    {
        Score.Value = model.Score;
        Record.Value = model.Record;
        IsSoundOff = model.IsSoundOff;
        TilesOnField = model.TilesOnField;
        IsModelInited = model.IsModelInited;
    }
}

[Serializable]
public class PlayerSaveModel
{
    public int Score;
    public int Record;
    public bool IsSoundOff;
    public List<TileSaveModel> TilesOnField;
    public bool IsModelInited;
}

public class Score: ObservableProperty<int>
{
}

public class Record : ObservableProperty<int>
{
}
