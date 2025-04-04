using Cysharp.Threading.Tasks;

public interface IAwaitableOnInit
{
    public UniTask Init();
}
