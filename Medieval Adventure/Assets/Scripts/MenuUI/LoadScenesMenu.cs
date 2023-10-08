using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LoadScenesMenu : MonoBehaviour
{
    GameStateMachine _gameStateMachine;

    [Inject]
    private void Construct(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    public void ContinueGame()
    {
        _gameStateMachine.ContinueGame();

    }
    public void LoadNewGame()
    {
        _gameStateMachine.LoadNewGame();

    }
    public void LoadMainMenu()
    {
        _gameStateMachine.LoadMainMenu();

    }


}
