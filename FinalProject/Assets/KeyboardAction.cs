using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class KeyboardAction : SerializedMonoBehaviour
{
    [SerializeField] private IRunable _runable; 

    private InputController _inputController;
    [Inject]
    private void Construct(InputController inputController)
    {
        _inputController = inputController;
    }
    private void Start()
    {
        _inputController.EnterIsPressed += RunAction;
    }
    private void OnDestroy()
    {
        _inputController.EnterIsPressed -= RunAction;
    }

    private void RunAction() {
        _runable.Run();
    }
}
