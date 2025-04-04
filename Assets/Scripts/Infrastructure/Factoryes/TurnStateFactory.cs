public class TurnStateFactory : ITurnStateFactory
{
    private readonly IResolver _resolver;

    public TurnStateFactory(IResolver diResolver) =>
      _resolver = diResolver;

    public T CreateState<T>() where T : ITurnState =>
      _resolver.Resolve<T>();
}
