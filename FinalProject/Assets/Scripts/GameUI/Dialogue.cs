using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class Dialogue : SerializedMonoBehaviour, IPointerClickHandler
{

    [SerializeField] private DialoguePanel _dialoguePanel;
    [SerializeField] private Image _illustrationImage;
    [SerializeField] private Text _text1;
    [SerializeField] private Text _text2;
    [SerializeField] protected GameUIData UISkin; 
    [SerializeField] private Image graphic1;
    [SerializeField] private Image graphic2;    
    [SerializeField] private Sprite _skinDefault;
    [SerializeField] private Sprite _skinMental;
    [SerializeField] private InteractiveGUIElement _interactiveGUIElement;
     private float _growthSpeed = 6f;
    private readonly float _textWriteSpeed = 25;
    private static  DialogueData _dialogueData;
    private static int _currentPhraseNum = 0;
    private static CharacterList _charactersList;
    private static InfoPanel _infoPanel;
    private float _disappearScale = 0.3f;
    private static  PhraseData _currentPhrase;
    private static string _currentCharacterName = "";
    private DialogueData _dialogueFork;
    private CardData _card;
    private string _reaction;
    [Inject]
    private void Construct(CharacterList characterList, InfoPanel infoPanel)
    {
        _charactersList = characterList;
        _infoPanel = infoPanel;
    }
    public void Activate(DialogueData dialogueData)
    {
        _dialogueData = dialogueData;
        _currentPhraseNum = 0;
        NextPhrase();

    }
    private void Start()
    {
        _charactersList.EndOfAnimation += NextPhrase;
    }

    private void OnDestroy()
    {
        _charactersList.EndOfAnimation -= NextPhrase;
    }

    public void NextPhrase()
    {
        gameObject.SetActive(true);
        if(_currentPhraseNum < _dialogueData.phrases.Length)
        {
            _currentPhrase = _dialogueData.phrases[_currentPhraseNum];
            SayPhrase(_currentPhrase);
            _currentPhraseNum++;
        }
        else
        {
            Hide();
        }
    }
    public void Fill(PhraseData phrase)
    {
        _dialogueFork = phrase.DialogFork;
        _card = phrase.Card;
        _reaction = phrase.Reaction;

    }

    private void SayPhrase(PhraseData phrase)
    {

        SetSkin(phrase);
        GameObject character = _charactersList.CharactersList[phrase.CharacterName];
        _dialoguePanel.DialogPosition = character.transform.Find("DialogPosition").transform;

        _illustrationImage.sprite = phrase.Illustration;
        if(_currentCharacterName != phrase.CharacterName)
        {
            Show();
        }
        Say(phrase);

        Fill(phrase);

        while(_dialogueData.phrases[_currentPhraseNum].AddSelection)
        {
            Dialogue newDialog = Instantiate(gameObject, transform.parent).GetComponent<Dialogue>();          
            _currentPhraseNum++;

            newDialog.Fill(_dialogueData.phrases[_currentPhraseNum]);
            if(_currentCharacterName != phrase.CharacterName)
            {
                newDialog.Show();
            }
            newDialog.Say(_dialogueData.phrases[_currentPhraseNum]);

        }
        _currentCharacterName = phrase.CharacterName;
    }
    public void Show()
    {
        KillAllTwins();
        transform.DOComplete();
        transform.localScale = Vector3.one * _disappearScale;
        transform.DOScale(1, _growthSpeed).SetSpeedBased().SetEase(Ease.Linear);
    }

    public void Say(PhraseData phrase)
    {
        _text1.DOKill();
        _text1.text = phrase.Phrase;
        Utils.Rebuild(_illustrationImage.transform);
        Utils.Rebuild(_text1.transform);
        _text2.DOKill();
        Utils.Rebuild(_text2.transform);
        _currentPhrase = phrase;
        if(phrase.Mental)
        {
            _text2.text = phrase.Phrase;
        }
        else
        {
            _text2.text = "";
            StartCoroutine(WhileTweening(StartWritingText));
        }
    }

    IEnumerator WhileTweening(Action action)
    {
        while(DOTween.IsTweening(transform))
        {
            yield return null;
        }
        action();
    }

    private void StartWritingText()
    {
        _text2.DOKill();
        _text2.text = "";
        Utils.Rebuild(_text2.transform);
        (_text2.transform as RectTransform).sizeDelta = (_text1.transform as RectTransform).sizeDelta;
        _text2.DOText(_text1.text, _textWriteSpeed).SetSpeedBased().SetEase(Ease.Linear).onComplete = Reaction;
    }


    private void Reaction() {
        if(transform.parent.childCount == 1 && _reaction != "")
        {
            gameObject.SetActive(false);
            _charactersList.PlayAnimation(_reaction, _dialogueData);
            _reaction = "";
        }
    }

    private void SetSkin(PhraseData phrase)
    {
        if(phrase.Mental)
        {
            graphic1.sprite = _skinMental;
            graphic2.enabled = false;
        }
        else
        {
            graphic1.sprite = _skinDefault;
            graphic2.enabled = true;
        }
    }


    public void KillAllTwins()
    {

        transform.DOKill();
        _text1.DOKill();
        _text2.DOKill();
    }

    public void Hide()
    {
        _currentCharacterName = "";
        KillAllTwins();
        transform.DOScale(_disappearScale, _growthSpeed).SetSpeedBased().SetEase(Ease.Linear).onComplete = HideComplete;
//        _interactiveGUIElement.SetDefaultState();
    }
    public void HideComplete()
    {
        _dialoguePanel.ClearExtraDialogs();
        _dialoguePanel.gameObject.SetActive(false);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        

        if(transform.parent.childCount > 1)
        {
            for(int i = 0; i < transform.parent.childCount; i++)
            {
                if(transform.parent.GetChild(i) != this.transform)
                {
                    Dialogue d = transform.parent.GetChild(i).GetComponent<Dialogue>();
                    d.KillAllTwins();
                    Destroy(d.gameObject);
                    
                }
            }
            graphic1.sprite = _skinDefault;
            graphic2.enabled = true;
            StartWritingText();
         }
        else
        {
            if(_dialogueFork != null)
            {
                _dialogueData = _dialogueFork;
                _currentPhraseNum = 0;
                _dialogueFork = null;
            }

            if(_card != null)
            {
                bool contains = _infoPanel.AddCard(_currentPhrase.Card, (transform.parent as RectTransform).position);
                if(contains)
                {
                    NextPhrase();
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if(_reaction != "")
            {
                Reaction();
            }
            else
            {
                NextPhrase();
            }
        }
         
    }
}
