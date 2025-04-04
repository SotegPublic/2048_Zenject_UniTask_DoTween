using System;

public class SpawTileState : BaseTurnState<IModelForSpawn>
{
    private ITilesSpawner _tilesSpawner;

    public SpawTileState(IModelForSpawn model, ITilesSpawner spawner) : base(model)
    {
        _tilesSpawner = spawner;
    }

    public override async void ProcessedState()
    {
        await _tilesSpawner.SpawnTile();
        _turnModel.OnStateEnd?.Invoke(this.GetType());
    }
}
