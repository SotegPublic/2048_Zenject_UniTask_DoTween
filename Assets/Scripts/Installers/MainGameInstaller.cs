using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class MainGameInstaller : MonoInstaller
{
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private GameBootstrapper _bootstrapper;
    [SerializeField] private Transform _poolTransform;
    [SerializeField] private TilesPoolConfig _poolConfig;
    [SerializeField] private GridGeneratorConfig _gridGeneratorConfig;
    [SerializeField] private TilesConfig _tilesConfig;
    [SerializeField] private TilesSpritesConfig _spritesConfig;

    public override void InstallBindings()
    {
        Container.Bind<GameBootstrapper>().FromInstance(_bootstrapper).AsSingle();
        InstallSoundManager();

        Container.BindInterfacesTo<InputHandler>().AsSingle();
        Container.Bind<IGameObjectFactory>().To<GameObjectFactory>().AsSingle();

        Container.BindInterfacesTo<GameAnalyticsSystem>().AsSingle();

        InstallPlayer();

        Container.Bind<ISaveSystem>().To<SaveSystem>().AsSingle();
        Container.Bind<IEndGameChecker>().To<EndGameChecker>().AsSingle();

        InstallGridSystem();
        InstallTilesSystem();
        InstallTurnSystem();
    }

    private void InstallTurnSystem()
    {
        Container.Bind<ITurnStateFactory>().To<TurnStateFactory>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TurnModel>().AsSingle().NonLazy();

        Container.Bind<TurnStateMachine>().AsSingle().NonLazy();
        Container.BindInterfacesTo<TurnStateMachine>().FromResolve();
        Container.Bind<IInitable>().WithId("turnController").To<TurnStateMachine>().FromResolve();

        Container.BindInterfacesAndSelfTo<AwaitInputState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ShowAdvState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CalculateMoveState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<MoveState>().AsSingle().WithArguments(_tilesConfig.TileMoveSpeed).NonLazy();
        Container.BindInterfacesAndSelfTo<MergeState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SpawTileState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CheckWinState>().AsSingle().NonLazy();
    }

    private void InstallPlayer()
    {
        Container.BindInterfacesTo<PlayerInfoHolder>().AsSingle();
        Container.BindInterfacesTo<PlayerRecordSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerModel>().AsSingle();
    }

    private void InstallSoundManager()
    {
        Container.Bind<SoundManager>().FromInstance(_soundManager).AsSingle();
        Container.BindInterfacesTo<SoundManager>().FromResolve();
        Container.Bind<IInitable>().WithId("soundManager").To<SoundManager>().FromResolve();
    }

    private void InstallTilesSystem()
    {
        Container.Bind<ITileViewUpdater>().To<TileViewUpdateSystem>().AsSingle().WithArguments(_spritesConfig);

        Container.Bind<TilesViewPool>().AsSingle().WithArguments(_gridGeneratorConfig.GridSize, _poolTransform.position, _poolConfig);
        Container.BindInterfacesTo<TilesViewPool>().FromResolve();
        Container.Bind<IAwaitableOnInit>().WithId("tilesPool").To<TilesViewPool>().FromResolve();

        Container.Bind<IFactory<Vector2, UniTask<TileView>>>().To<TileFactory>().AsSingle().WithArguments(_tilesConfig.TileRef);
        Container.BindInterfacesTo<TilesSpawner>().AsSingle();
    }

    private void InstallGridSystem()
    {
        Container.Bind<GridGenerator>().AsSingle().WithArguments(_gridGeneratorConfig).NonLazy();
        Container.BindInterfacesTo<GridGenerator>().FromResolve();
        Container.Bind<IAwaitableOnInit>().WithId("gridGenerator").To<GridGenerator>().FromResolve();
    }
}