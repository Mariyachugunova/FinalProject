using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private MovementController _playerMovementController;
    [SerializeField] private ArrowIcone _arrowIconePrefab;
    [SerializeField] private CharacterList _characterList;
    [SerializeField] private InfoPanel _infoPanel;
    [SerializeField] private PauseController _pauseController;
    [SerializeField] private GlobalObjects _globalObjects;
    [SerializeField] private GameStateMachine _gameStateMachine;
    [SerializeField] private DialoguePanel _dialoguePanel;
    [SerializeField] private InputController _inputController;
    [SerializeField] private Path _path;
    [SerializeField] private CameraTurn _cameraTurn;
    public override void InstallBindings()
    {
        Container.Bind<MovementController>().FromInstance(_playerMovementController).AsSingle().NonLazy();
        Container.QueueForInject(_playerMovementController);

      
        Container.Bind<ArrowIcone>().FromInstance(_arrowIconePrefab).AsSingle();
        Container.QueueForInject(_arrowIconePrefab);

        Container.Bind<CharacterList>().FromInstance(_characterList).AsSingle();
        Container.QueueForInject(_characterList);

        Container.Bind<PauseController>().FromInstance(_pauseController).AsSingle();
        Container.QueueForInject(_pauseController);

        Container.Bind<GlobalObjects>().FromInstance(_globalObjects).WithConcreteId("LoadingPanel");
        Container.QueueForInject(_globalObjects);

        Container.Bind<GameStateMachine>().FromInstance(_gameStateMachine).AsSingle().NonLazy();
        Container.QueueForInject(_gameStateMachine);

        Container.Bind<InfoPanel>().FromInstance(_infoPanel).AsSingle().NonLazy();
        Container.QueueForInject(_infoPanel);

        Container.Bind<DialoguePanel>().FromInstance(_dialoguePanel).AsSingle().NonLazy();
        Container.QueueForInject(_dialoguePanel);

        Container.Bind<InputController>().FromInstance(_inputController).AsSingle().NonLazy();
        Container.QueueForInject(_inputController);

        Container.Bind<Path>().FromInstance(_path).AsSingle().NonLazy();
        Container.QueueForInject(_path);

        Container.Bind<CameraTurn>().FromInstance(_cameraTurn).AsSingle().NonLazy();
        Container.QueueForInject(_cameraTurn);


    }
}