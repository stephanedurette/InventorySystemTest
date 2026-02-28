using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private UIElementFactory uiElementFactory;
    [SerializeField] private UIObjectPooler uiObjectPooler;

    public override void InstallBindings()
    {
        Container.Bind<UIElementFactory>().FromInstance(uiElementFactory).AsSingle();
        Container.Bind<UIObjectPooler>().FromInstance(uiObjectPooler).AsSingle();
    }
}
