public class StateFactory: IStateFactory
{
    private readonly IResolver _resolver;

    public StateFactory(IResolver diResolver) =>
      _resolver = diResolver;

    public T CreateState<T>() where T : IGameState =>
      _resolver.Resolve<T>();
}
