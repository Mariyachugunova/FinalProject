using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class Card : MonoBehaviour, IPointerClickHandler
{

    private int _id;
    public int Id => _id;

    [SerializeField] Image _BackgroundImage;
    [SerializeField] Text _text;
    [SerializeField] Image _illustrationImage;
    [SerializeField] private Dictionary<string, DialogueData> _dialogs;

    private static CharacterList _charactersList;
    private static InfoPanel _infoPanel;
    private DialoguePanel _dialoguePanel;
    private CardData _cardData;
    public int MyProperty { get; set; }

    [Inject]
    private void Construct(CharacterList characterList, InfoPanel infoPanel,DialoguePanel dialoguePanel)
    {
        _charactersList = characterList;
        _infoPanel = infoPanel;
        _dialoguePanel = dialoguePanel;
    }

    private void Awake()
    {

    }
    public void UnFade()
    {
        _BackgroundImage.color = new Color(1, 1, 1, 0);
        _BackgroundImage.DOFade(1f, 1f);
        _illustrationImage.color = new Color(1, 1, 1, 0);
        _illustrationImage.DOFade(1f, 1f);


    }

    public void OnPointerClick(PointerEventData eventData)
    {
    

        if(_cardData == null)
        {
            _infoPanel.GetCard();
        }
        else
        {
            Turn();
            
        }

        if(transform.parent?.parent?.name == "InfoPanel")
        {
            if(_dialogs != null)
            {
                foreach(var item in _dialogs)
                {
                    print(item.Key + " " + item.Value);
                }

                //_dialoguePanel.CurrentDialog.Activate()
            }
        }
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


}
