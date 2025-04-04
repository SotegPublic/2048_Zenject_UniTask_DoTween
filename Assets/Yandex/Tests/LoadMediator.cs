#if UNITY_EDITOR
using System;
public class LoadMediator: ILoadMediator
{
    public Action<string> OnJsonLoad { get; set; }
    public void WhenJsonLoad(string json)
    {
        OnJsonLoad?.Invoke(json);
    }
}

public interface ILoadMediator
{
    public Action<string> OnJsonLoad { get; set; }
    public void WhenJsonLoad(string json);
}
#endif
