using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Zenject;

public class InitYandexState : BaseState, IDisposable
{
    private IYandexSystem _yandex;

    private CancellationTokenSource _tokenSource = new CancellationTokenSource();

    [Inject]
    public InitYandexState(IYandexSystem yandexSystem)
    {
        _yandex = yandexSystem;
    }

    public override async void EnterState()
    {
        if (_yandex.IsYandexInit())
        {
            OnStateEnd?.Invoke(this.GetType());
        }
        else
        {
            await UniTask.WaitUntil(IsYandexInit, cancellationToken: _tokenSource.Token);
            OnStateEnd?.Invoke(this.GetType());
        }
    }

    public bool IsYandexInit()
    {
        return _yandex.IsYandexInit();
    }

    public void Dispose()
    {
        _tokenSource?.Cancel();
        _tokenSource.Dispose();
    }

    public override void ExitState()
    {
    }
}
