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

public class InfoPanel : MonoBehaviour
{

    [SerializeField] private Transform _leftPack;
    [SerializeField] private Transform _rightPack;
    [SerializeField] private RectTransform _leftToRight;
    [SerializeField] private RectTransform _rightToLeft;
    [SerializeField] private Transform _centerPack;
    [SerializeField] private Ease _ease;
    [SerializeField] private GameObject _closeTrigger;
    [SerializeField] Card cardPrefab;
    private float _speed1 = 350;
    private float _speed2 = 2500;
    private float _speedOfGrowth = 3;
    private float _cardWidht = 256;
    private PointerEventData _pointerData;
    private List<RaycastResult> _raycastResult;
    private float _space = 12;
    private float _spaceCenter = 12;
    private Transform _card;
    private float _getCartDuration = 0.4f;
    private Transform LastCardLeft { get { return _leftPack.GetChild(_leftPack.childCount - 1); } }
    private Transform FirstCardRight => _rightPack.GetChild(_rightPack.childCount - 1);
    private DialoguePanel _dialoguePanel;
    [Inject]
    private void Construct(DialoguePanel dialoguePanel)
    {
        _dialoguePanel = dialoguePanel;
    }
    public List<Card> AllCards
    {
        get
        {
            List<Card> cards = new();
            cards.Clear();

            for(int i = 0; i < _leftPack.childCount; i++)
            {
                cards.Add(_leftPack.GetChild(i).GetComponentInChildren<Card>());
            }
            for(int i = 0; i < _centerPack.childCount; i++)
            {
                cards.Add(_centerPack.GetChild(i).GetComponentInChildren<Card>());
            }
            for(int i = _rightPack.childCount - 1; i >= 0; i--)
            {
                cards.Add(_rightPack.GetChild(i).GetComponentInChildren<Card>());
            }
            return cards;
        }
    }
    void Start()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        _pointerData = new PointerEventData(EventSystem.current);
        _raycastResult = new List<RaycastResult>();
        Fold();
    }



    [Button]
    public void MoveRight()
    {
        if(_leftPack.childCount > 1 && !DOTween.IsTweening(_centerPack))
        {

            float x = _leftPack.GetChild(_leftPack.childCount - 2).position.x + _cardWidht + _spaceCenter;
            PutLastLeftCardAtCenter();
            _centerPack.DOMoveX(x, _speed1).SetSpeedBased().SetEase(_ease).onComplete = DropRight;

        }
    }

    [Button]
    public void MoveLeft()
    {

        if(_rightPack.childCount > 1 && !DOTween.IsTweening(_centerPack))
        {
            PutFirstCardRightAtCenter();
            float x = LastCardLeft.position.x + _space;
            _centerPack.DOMoveX(x, _speed1).SetSpeedBased().SetEase(_ease).onComplete = DropLeft;


        }
    }

    private void DropRight()
    {
        Transform lastCardCenter = _centerPack.GetChild(_centerPack.childCount - 1);
        lastCardCenter.SetParent(_rightPack);
        _rightToLeft.sizeDelta = new Vector2(_rightPack.childCount * _space, _rightToLeft.rect.height);

    }
    private void DropLeft()
    {
        if(_centerPack.childCount > 0)
        {
            Transform firstCardCenter = _centerPack.GetChild(0);
            if(_centerPack.childCount > 1)
            {
                Vector3 newPosition = new Vector3(_centerPack.GetChild(1).position.x, _centerPack.position.y, _centerPack.position.z);
                _centerPack.position = newPosition;
            }
            firstCardCenter.SetParent(_leftPack);
            _leftToRight.sizeDelta = new Vector2(_leftPack.childCount * _space, _leftToRight.rect.height);

        }

    }

    void FixedUpdate()
    {
        Mouse mouse = Mouse.current;

        _pointerData.position = mouse.position.ReadValue();
        EventSystem.current.RaycastAll(_pointerData, _raycastResult);

        bool hitLeft = false;
        bool hitRight = false;
        _raycastResult.ForEach((result) => {
            if(result.gameObject == _leftToRight.gameObject) hitLeft = true;
            if(result.gameObject == _rightToLeft.gameObject) hitRight = true;
        });
        if(hitLeft)
        {
            if(_centerPack.childCount == 0)
            {
                Growth();
            }
            else
            {
                MoveRight();
            }
        }
        if(hitRight)
        {
            MoveLeft();
        }

    }
    [Button]

    public void Fold()
    {
        if(_centerPack.childCount > 0)
        {
            if(_rightPack.childCount > 0)
            {
                PutFirstCardRightAtCenter();
            }
            float x = _leftPack.position.x + _leftPack.childCount * _space;
            _centerPack.DOMoveX(x, _speed2).SetSpeedBased().SetEase(_ease).onComplete = FoldEnd;
            transform.DOScale(0.4f, _speedOfGrowth).SetSpeedBased().SetEase(_ease);
        }
        else
        {
            DropLeft();
            _leftToRight.sizeDelta = new Vector2(_leftPack.childCount * _space + _cardWidht, _leftToRight.rect.height);
            transform.DOScale(0.4f, _speedOfGrowth).SetSpeedBased().SetEase(_ease);
            _closeTrigger.gameObject.SetActive(false);
        }

    }

    private void FoldEnd()
    {

        DropLeft();
        Fold();
    }

    [Button]
    public void Growth()
    {
        if(!DOTween.IsTweening(transform))
            transform.DOScale(1, _speedOfGrowth).SetSpeedBased().SetEase(_ease).onComplete = Put;

    }

    [Button]
    public void Put()
    {

        if(_leftPack.childCount > 1 && !DOTween.IsTweening(_centerPack))
        {
            SetCenterPackWidht();
            if(_rightPack.childCount == 0)
            {
                PutLastLeftCardAtCenter();
                _centerPack.DOMoveX(LastCardLeft.position.x + _cardWidht + _spaceCenter, _speed2).SetSpeedBased().SetEase(_ease).onComplete = PutEnd;
                _closeTrigger.gameObject.SetActive(true);
            }
        }

    }

    private void PutEnd()
    {
        if(_centerPack.childCount == 6)
        {
            DropRight();
        }
        Put();

    }

    [Button]
    private void PutLastLeftCardAtCenter()
    {

        _centerPack.position = LastCardLeft.position;
        LastCardLeft.SetParent(_centerPack);
        _centerPack.GetChild(_centerPack.childCount - 1).SetAsFirstSibling();
        _leftToRight.sizeDelta = new Vector2(_leftPack.childCount * _space + _space, _leftToRight.rect.height);
    }
    private void PutFirstCardRightAtCenter()
    {
        FirstCardRight.SetParent(_centerPack);
        _rightToLeft.sizeDelta = new Vector2(_rightPack.childCount * _space, _rightToLeft.rect.height);
    }

    [Button]
    public bool AddCard(CardData card, Vector2 position)
    {
        
        position = new Vector2(position.x - _cardWidht / 2, position.y);
        bool contains = false;
        var allCards = AllCards;
        for(int i = 0; i < allCards.Count; i++)
        {
            if(allCards[i].Id == card.Id)
            {
                if(card.Answer)
                {
                    InstantiateAnswerCard(allCards[i], card, position);
                }
                else
                {
                    contains = true;
                }
            }                
        }
        
        if(!contains && !card.Answer)
        {   
            InstantiateCard(card, position);
        }
        return contains;
    }
    void InstantiateCard(CardData cardData, Vector2 position)
    {
        Card card = Instantiate(cardPrefab, transform.parent);
        card.SetData(cardData);
        _card = card.transform;
        card.UnFade();
        (card.transform as RectTransform).anchoredPosition = position;
        _leftToRight.sizeDelta = new Vector2(_leftPack.childCount * _space + _cardWidht, _leftToRight.rect.height);

    }

    void InstantiateAnswerCard(Card card, CardData cardData, Vector2 position)
    {     
        Card newcard = Instantiate(card, card.transform.position,Quaternion.identity, transform.parent);
        newcard.SetAnswer(cardData);
        newcard.transform.localScale = new Vector2(0.4f, 0.4f);
        newcard.transform.DOMove(position, 1);
        newcard.transform.DOScale(1, 1); 
        _card = newcard.transform;
        Destroy(card.gameObject);
    }


    [Button]
    public void GetCard()
    {
        if(_card != null)
        {        
            _card.DOScale(0.4f, _getCartDuration).SetEase(_ease);
            _card.DOMove(LastCardLeft.position, _getCartDuration).SetEase(_ease).onComplete = PutCardInPack;
        
        }
    }
   

    private void PutCardInPack()
    {
        _card.SetParent(_leftPack);
        _card = null;
        _dialoguePanel.CurrentDialog.NextPhrase();
    }

    [Button]
    private void RemoveCard(int n)
    {
        Fold();
        StartCoroutine(DestroyCard(n));
    }
    IEnumerator DestroyCard(int n)
    {
        while(_centerPack.childCount >0)
        {
            yield return null;
        }
        Destroy(AllCards[n].gameObject);
        AllCards.RemoveAt(n);
    }
  


    [Button]
    private void SetCenterPackWidht()
    {
        var layoutGrope = _centerPack.GetComponent<HorizontalLayoutGroup>();
        int cardCount = transform.GetComponentsInChildren<Card>().Length;
        
        if(_leftPack.childCount > 6) {
            float w =_rightPack.position.x - _leftPack.position.x - _space * (cardCount - 7) - _cardWidht;
            _spaceCenter = ( w  - _cardWidht*5) / 6;
           
        }
        layoutGrope.spacing = _spaceCenter;
    }

    [Button]

    public void Show()
    {

    }

}



