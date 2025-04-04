using Zenject;

public class DIResolver: IResolver
{
    private readonly DiContainer _container;

    public DIResolver(DiContainer diContainer)
    {
        _container = diContainer;
    }

    public T Resolve<T>()
    {
        return _container.Resolve<T>();
    }
}
