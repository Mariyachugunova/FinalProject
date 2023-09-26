using UnityEngine;
using Zenject;

public class PauseController:MonoBehaviour
{
    private bool _pause;
    public bool Pause => _pause;


    private void Start()
    {
        _pause = false;
        gameObject.SetActive(false);
    }


    public void SetPause()
    {
        if(!_pause)
        {
            gameObject.SetActive(true);
            _pause = true;
        }
        else
        {
            gameObject.SetActive(false);
            _pause = false;
        }

 
    }

    public void MainMenuLoad()
    {
  
    }


}