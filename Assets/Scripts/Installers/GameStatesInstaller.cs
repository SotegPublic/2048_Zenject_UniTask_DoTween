using Zenject;

public class GameStatesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IResolver>().To<DIResolver>().AsSingle().NonLazy();
        Container.Bind<IStateFactory>().To<StateFactory>().AsSingle().NonLazy();

        Container.BindInterfacesTo<CurrentGameStateHolder>().AsSingle().NonLazy();
        Container.BindInterfacesTo<GameStateMachine>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<InitYandexState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerAuthState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LoadPlayerState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LoadLevelState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameInProgressState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ResetGameState>().AsSingle().NonLazy();
    }
}
