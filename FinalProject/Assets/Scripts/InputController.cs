using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputController : MonoBehaviour
{

    public event Action EnterIsPressed;
    private InputControls _inputControls;
    private MovementController _playerMovementController;
    private PauseController _pauseController;
    private DialoguePanel _dialoguePanel;
    private static InfoPanel _infoPanel;
    private Path _path;
    [SerializeField] private bool _keyboardControl;
    public bool KeyboardControl => _keyboardControl; 
    [Inject] 
    private void Construct(MovementController playerMovementController, PauseController pauseController, DialoguePanel dialoguePanel, Path path, InfoPanel infoPanel)
    {
        _playerMovementController = playerMovementController;
        _pauseController = pauseController;
        _dialoguePanel = dialoguePanel;
        _path = path;
        _infoPanel = infoPanel;
    }

    private void Awake()
    {
        _inputControls = new InputControls();
        _inputControls.Main.Q.performed += context => QPerformed();
        _inputControls.Main.AnyKey.performed += context => EnyKey();
        _inputControls.Main.Space.performed += context => Enter();        
        _inputControls.Main.Enter.performed += context => Enter();
        _inputControls.Main.Up.performed += context => Up();
        _inputControls.Main.Down.performed += context => Down();
        _inputControls.Main.Left.performed += context => Left();
        _inputControls.Main.Right.performed += context => Right();

    }

    private void QPerformed()
    {
        _infoPanel.QPerformed();
    }

    private void EnyKey()
    {
        
    }

    private void Enter()
    {
        if(!_dialoguePanel._close)
        {
            _dialoguePanel.DialogueContinue();
        }
        EnterIsPressed.Invoke();
    }

    private void Down()
    {
        _dialoguePanel.Down();
    }

    private void Up()
    {
        _dialoguePanel.Up();
    }
    private void Right()
    {
        if(!_infoPanel.Collapsed)
        {
            _infoPanel.SelectRightCard();
           
        }
    }

    private void Left()
    {
        if(!_infoPanel.Collapsed)
        {
            _infoPanel.SelectLeftCard();
        }
    }

    private void Pause()
    {
        _pauseController.SetPause();
    }
     

    private void OnEnable()
    {
        _inputControls.Enable();
    }
    private void OnDisable()
    {
        _inputControls.Disable();
    }
    private Vector2 _lastMousePosition;
    private void Update()
    {
        Vector2 direction = _inputControls.Main.Move.ReadValue<Vector2>();
        OnMove(direction);

        if(_lastMousePosition != Mouse.current.position.value) _keyboardControl = false;
        _lastMousePosition = Mouse.current.position.value;
        if(Keyboard.current.anyKey.wasPressedThisFrame)_keyboardControl = true;
    }
    public void OnMove(Vector2 inputVec)
    {
        if(inputVec.y > 0)
        {
            if(!_dialoguePanel.Selection && _dialoguePanel.CanGo() && _playerMovementController.CanGo() && _infoPanel.Collapsed)
            { 
                _dialoguePanel.Close();
                _playerMovementController.MoveTo(Direction.up);
            }
        }
        else
        if(inputVec.y < 0)
        {
            if(!_dialoguePanel.Selection && _dialoguePanel.CanGo() && _playerMovementController.CanGo()&&_infoPanel.Collapsed)
            {
                if(!_dialoguePanel._close) _dialoguePanel.Close();
                _playerMovementController.MoveTo(Direction.down);
            }
            
        }
        else
        if(inputVec.x > 0)
        {
            if(_dialoguePanel.CanGo() && _playerMovementController.CanGo() && _infoPanel.Collapsed)
            { 
                _dialoguePanel.Close();
                _playerMovementController.MoveTo(Direction.right);
            }
            
        }
        else
        if(inputVec.x < 0)
        {
            if(_dialoguePanel.CanGo() && _playerMovementController.CanGo() && _infoPanel.Collapsed)
            {
                _dialoguePanel.Close();
                _playerMovementController.MoveTo(Direction.left);
            }
            
        }


        if(inputVec == Vector2.zero)
        {
            _playerMovementController.StopKeyboardMove();
        }
        else
        {
            _playerMovementController.StopMouseMove();
            _path.HideArrow();
          
        }
    }


}
