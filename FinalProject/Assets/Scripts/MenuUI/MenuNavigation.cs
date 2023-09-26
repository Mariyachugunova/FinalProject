using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class MenuNavigation : MonoBehaviour
{

    private InputControls _inputControls;
    private Selectable _selectedButton;
    private Selectable[] _selectables;
    private MainMenuPanels _mainMenuPanels;

    [Inject]
    private void Construct(MainMenuPanels mainMenuPanels)
    {
        _mainMenuPanels = mainMenuPanels;

    }

    private void Awake()
    {
        _inputControls = new InputControls();
        _inputControls.Main.Up.performed += context => Up();
        _inputControls.Main.Down.performed += context => Down();
        _inputControls.Main.Enter.performed += context => Enter();
        _selectables = GetComponentsInChildren<Selectable>();
        _inputControls.Main.Esc.performed += context => Back();
    }

    private void Back()
    {
        _mainMenuPanels.ShowMainMenuPanel();
    }

    private void Enter()
    {
        _selectedButton = EventSystem.current?.currentSelectedGameObject?.GetComponent<Selectable>();
        (_selectedButton as Button)?.onClick?.Invoke();
        print(_selectedButton);
    }
    private void Down()
    {
        _selectedButton = EventSystem.current?.currentSelectedGameObject?.GetComponent<Selectable>();
         
        if(_selectedButton == null)
        {    
            _selectedButton = _selectables[_selectables.Length - 1];       
            _selectedButton.Select();

        }

    }

    private void Up()
    {
        _selectedButton = EventSystem.current?.currentSelectedGameObject?.GetComponent<Selectable>();
        
        if(_selectedButton == null)
        {
            _selectedButton = _selectables[0];
            _selectedButton.Select();
        }

    }

    private void OnEnable()
    {
        _inputControls.Enable();
    }
    private void OnDisable()
    {
        _inputControls.Disable();
    }
    
}
