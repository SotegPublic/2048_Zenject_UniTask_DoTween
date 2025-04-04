using System;
using UnityEngine;
using Zenject;

public class YandexInstaller : MonoInstaller
{
    [SerializeField] private YandexReceiver _yandexReceiver;

    public override void InstallBindings()
    {
        Container.Bind<YandexReceiver>().FromInstance(_yandexReceiver).AsSingle();

        InstallNotifier();

        Container.BindInterfacesTo<SaveAndLoadDataProvider>().AsSingle();
        Container.Bind<IYandexSystem>().To<YandexSysetem>().AsSingle();
        Container.Bind<IShowAdvSystem>().To<ShowAdvSystem>().AsSingle();
    }

    private void InstallNotifier()
    {
        Container.BindInterfacesTo<YandexAdvMediator>().AsSingle();
        Container.Bind<YandexCallbacksNotifier>().AsSingle();
        Container.Bind<IYandexCallbacksNotifier>().To<YandexCallbacksNotifier>().FromResolve();
        Container.Bind<IDisposable>().To<YandexCallbacksNotifier>().FromResolve();

#if UNITY_EDITOR
        InstallBindingsForTests();
#endif
    }

#if UNITY_EDITOR
    private void InstallBindingsForTests()
    {
        Container.Bind<ILoadMediator>().To<LoadMediator>().AsSingle();
        Container.BindInterfacesTo<YandexSystemInEditorMediator>().AsSingle();
        Container.Bind<IInEditorYandexCallbacksNotifier>().To<YandexCallbacksNotifier>().FromResolve();
    }
#endif
}