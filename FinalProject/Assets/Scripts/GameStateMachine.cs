using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameStateMachine : MonoBehaviour
{

    private Image _loadingPanel;
    private float _fadeDuration = 0.5f;
    [Inject]
    private void Construct(GlobalObjects globalObjects)
    {
        _loadingPanel = globalObjects.LoadingPanel;
    }

    private Animator _animator;
    private void Start() {
        _animator = GetComponent<Animator>();
        HideLoadindPanel();

    }

    [Button]
    private void HideLoadindPanel()
    {
        _loadingPanel.DOComplete();
        _loadingPanel.color = Color.black;
        _loadingPanel.gameObject.SetActive(true);
        _loadingPanel.DOFade(0, _fadeDuration).onComplete = DeactivateLoadingPanel;

    }
    private void DeactivateLoadingPanel()
    {
    _loadingPanel.gameObject.SetActive(false);
    }

    [Button]
    private void ShowLoadindPanel()
    {
        _loadingPanel.DOComplete();
        _loadingPanel.color = new Color(0,0,0,0); 
        _loadingPanel.gameObject.SetActive(true);
        _loadingPanel.DOFade(1, _fadeDuration);

    }

    private string _sceneName;
    private void LoadSceneStart(string sceneName)
    {
        _sceneName = sceneName;
 
        _loadingPanel.DOComplete();
        _loadingPanel.color = new Color(0, 0, 0, 0);
        _loadingPanel.gameObject.SetActive(true);
        _loadingPanel.DOFade(1, _fadeDuration).onComplete = LoadSceneEnd;

    }
    private void LoadSceneEnd()
    {
        SceneManager.LoadScene(_sceneName);
    }

    [Button]
    private void LoadZombiesScene() {
        LoadSceneStart("ZombiesScene");
    }
    [Button]
    private void LoadAsincZombiesScene()
    {
        StartCoroutine(Load("ZombiesScene"));
    }
    [Button]
    public void LoadMainMenu()
    {
        LoadSceneStart("MainMenu");
    }

    public void ContinueGame()
    {
        LoadSceneStart("Scene 1");
    }
    [Button]
    public void LoadNewGame()
    {
        LoadSceneStart("Scene 1");
    }


    private AsyncOperation _asincOperation;
    [Button]
    IEnumerator Load(string sceneName) {     
        _asincOperation = SceneManager.LoadSceneAsync(sceneName);
        while(!_asincOperation.isDone) {
            yield return null;
        }
    }
    
}
