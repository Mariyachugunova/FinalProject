using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class Card : InteractiveGUIElement, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IRunable
{

    private int _id;
    public int Id => _id;

    [SerializeField] Image _BackgroundImage;
    [SerializeField] Text _text;
    [SerializeField] Image _illustrationImage;
    [SerializeField] private Dictionary<string, DialogueData> _dialogs;
    private CardData _cardData;
    public CardData AnswerData => _cardData;
    public bool Qwestion => AnswerData != null;
    private static CharacterList _charactersList;
    private static InfoPanel _infoPanel;
    private DialoguePanel _dialoguePanel;
    private Character _playerCharacter;
    private InputController _inputController;
    public int MyProperty { get; set; }

    [Inject]
    private void Construct(CharacterList characterList, InfoPanel infoPanel,DialoguePanel dialoguePanel, Character playerCharacter, InputController inputController)
    {
        _charactersList = characterList;
        _infoPanel = infoPanel;
        _dialoguePanel = dialoguePanel;
        _playerCharacter = playerCharacter;
        _inputController = inputController;
    }
   
    public void UnFade()
    {
        RayCast();
        _BackgroundImage.color = new Color(_BackgroundImage.color.r, _BackgroundImage.color.g, _BackgroundImage.color.b, 0);
        _BackgroundImage.DOFade(1f,6f);
        _illustrationImage.color = new Color(_illustrationImage.color.r, _illustrationImage.color.g, _illustrationImage.color.b, 0);
        _illustrationImage.DOFade(1f, 6f);
       
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        _infoPanel.GetNewCard();

        if(inPack())
        {
            if(_dialogs != null)
            {
                string key = _dialoguePanel.CurrentDialog.CurrentInterlocutor;
                if(_dialogs.Keys.Contains(key))
                {

                    if(_dialoguePanel.CurrentDialog.CurrentDialogueData != _dialogs[key])
                    {
                        _dialoguePanel.CurrentDialog.Activate(_dialogs[key]);
                    }
                    else
                    {
                        //if(!_charactersList.CharactersList[_dialoguePanel.CurrentDialog.CurrentInterlocutor]._isAnimationPlaing)
                        {
                            if(!_dialoguePanel.gameObject.activeSelf)
                            {
                                _dialoguePanel.CurrentDialog.Activate(_dialoguePanel.CurrentDialog.CurrentDialogueData);

                            }
                            else
                            {
                                _dialoguePanel.CurrentDialog.DialogueProcess();
                            }
                        }

                    }
                }


            }
            else
            {
                _dialoguePanel.Close();
            }
        }
    }

    private bool inPack()
    {
        return transform.parent?.parent?.name == "InfoPanel";
    }

    public void Turn()
    {
        _BackgroundImage.transform.DOScaleX(0, 0.4f).SetEase(Ease.InCubic).onComplete = SetAnswerData;
  
    }

    private void SetAnswerData()
    {
        SetData(_cardData);
        _BackgroundImage.transform.transform.DOScaleX(1, 0.5f).SetEase(Ease.OutCubic);
        _cardData = null;
    }

    public void SetData(CardData card)
    {
       _id = card.Id;
       _text.text = card.TextContent;
       _BackgroundImage.sprite = card.BackgroundImage;
       _illustrationImage.sprite = card.IllustrationImage;
       _dialogs = card.Dialogs;
    }
    public void SetAnswer(CardData card)
    {
        _cardData = card;
    }
    public void DOComplete()
    {
        transform.DOComplete();
        _BackgroundImage.DOComplete();
        _illustrationImage.DOComplete();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //transform.parent.GetComponent<DialoguePanel>()?.DeselectAll();
        if (!_inputController.KeyboardControl) Select();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Deselect();
        _infoPanel._selectedCardNum = -1;
    }

    public void Run()
    {
       if (_selectedElement == this) OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
