using UnityEngine;
using Zenject;

public class MenuSceneInstaller : MonoInstaller
{
    [SerializeField] private GameStateMachine _gameStateMachine;
    [SerializeField] private GlobalObjects _globalObjects;
    [SerializeField] private MainMenuPanels _mainMenuPanels;
    public override void InstallBindings()
    {
        Container.Bind<GameStateMachine>().FromInstance(_gameStateMachine).AsSingle().NonLazy();
        Container.QueueForInject(_gameStateMachine);

        Container.Bind<GlobalObjects>().FromInstance(_globalObjects).AsSingle().NonLazy();
        Container.QueueForInject(_globalObjects);

        Container.Bind<MainMenuPanels>().FromInstance(_mainMenuPanels).AsSingle().NonLazy();
        Container.QueueForInject(_mainMenuPanels);

    }
}