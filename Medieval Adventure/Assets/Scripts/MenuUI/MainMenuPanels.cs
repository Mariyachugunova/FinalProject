using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPanels : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _settingsPanel;
    private void Start()
    {
        HideAll();
        ShowPanel(_mainPanel);

    }

    public void ShowMainMenuPanel(){
        ShowPanel(_mainPanel);
    }
    public void ShowSettingsPanel()
    {
        ShowPanel(_settingsPanel);
    }

    public void ShowPanel(GameObject panel)
    {
        HideAll();
        panel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(panel);
    }
    
    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);     

    }

    private void HideAll()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }



    public void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
                Application.Quit();

    }



}
