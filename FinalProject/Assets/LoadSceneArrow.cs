using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class LoadSceneArrow : MonoBehaviour,IPointerClickHandler,IRunable
{
    [SerializeField] private string _sceneName;
    [SerializeField] private InteractiveArrowIcon _ArrowIcone;
    private GameStateMachine _gameStateMachine;
    [Inject]
    private void Construct(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }
   
    public void LoadScene()
    {
        if(_ArrowIcone.IsSelected)
        {
            _gameStateMachine.LoadSceneStart(_sceneName);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LoadScene();
    }
    public void Run()
    {
        LoadScene();
    }

}
