using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputController : MonoBehaviour
{

    private InputControls _inputControls;

    private MovementController _playerMovementController;
    private PauseController _pauseController;
    private Animator _playerAnimator;
    [Inject] 
    private void Construct(MovementController playerMovementController, PauseController pauseController)
    {
        _playerMovementController = playerMovementController;
        _pauseController = pauseController;
        _playerAnimator = _playerMovementController.GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        _inputControls = new InputControls();
        // _inputControls.Main.Up.performed += context => Up();
        //  _inputControls.Main.Down.performed += context => Down();
        // _inputControls.Main.Enter.performed += context => Enter();
        _inputControls.Main.Esc.performed += context => Pause();
    }

    private void Pause()
    {
        _pauseController.SetPause();
    }

    private void Jump()
    {
        print("Jump");
    }

    private void OnEnable()
    {
        _inputControls.Enable();
    }
    private void OnDisable()
    {
        _inputControls.Disable();
    }

    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        Vector3 moveVec = new Vector3(inputVec.x, 0, inputVec.y);

    }

    [Button]
    private void TestPlayer()
    {
      //print( );
    }
}
