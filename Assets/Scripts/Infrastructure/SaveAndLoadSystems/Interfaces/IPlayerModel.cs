using System.Collections.Generic;

public interface IPlayerModel
{
    public Score Score { get; set; }
    public Record Record { get; set; }
    public bool IsSoundOff { get; set;}
    public bool IsModelInited { get; set; }
    public List<TileSaveModel> TilesOnField { get; set; }
}
