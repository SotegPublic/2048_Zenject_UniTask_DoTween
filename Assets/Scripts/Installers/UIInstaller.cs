using System;
using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private InGameUIView _inGameUIView;
    [SerializeField] private LoadingScreenView _loadingScreenView;
    [SerializeField] private TextHolder[] _textHolders;
    public override void InstallBindings()
    {
        InstallUI();
        InstallTranslateSystem();
    }

    private void InstallTranslateSystem()
    {
        Container.Bind<ILanguageController>().To<LanguageController>().AsSingle().WithArguments(_textHolders);
    }

    private void InstallUI()
    {
        Container.Bind<InGameUIView>().FromInstance(_inGameUIView).AsSingle();
        Container.BindInterfacesTo<InGameUIViewController>().AsSingle().NonLazy();

        Container.Bind<LoadingScreenView>().FromInstance(_loadingScreenView).AsSingle();
        Container.BindInterfacesTo<LoadingScreenViewController>().AsSingle().NonLazy();
    }
}