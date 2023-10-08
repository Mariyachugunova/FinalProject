using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class InteractiveGUIElement : MonoBehaviour, IPointerClickHandler ,IPointerEnterHandler,IPointerExitHandler
{

    [SerializeField] protected GameUIData _UISkin;
    [SerializeField] private bool _multiselect = true;
    Image[] _spriteRenderer;
    private Color _normalColor;
    private Color _highlightedColor;
    private Vector3 _normalScale;
    private Vector3 _selectedScale;
    private float _scaleFactor;
    private float _transitionDuration;
    private bool _selected;
    public bool Selected => _selected;
    private PointerEventData _pointerData;
    private List<RaycastResult> _raycastResult;

    private void Awake()
    {
        _spriteRenderer = GetComponentsInChildren<Image>();

    }


    private void Start()
    {
        _normalColor = _UISkin.normalColor;
        _highlightedColor = _UISkin.highlightedColor;
        _scaleFactor = _UISkin.scaleFactor;
        _transitionDuration = _UISkin.transitionDuration;
        _normalScale = Vector3.one;
        _selectedScale = _normalScale*_scaleFactor;
        SetDefaultState();
        _pointerData = new PointerEventData(EventSystem.current);
        _raycastResult = new List<RaycastResult>();
    }
    public void SetDefaultState()
    {
        for(int i = 0; i < _spriteRenderer.Length; i++)
        {
             _spriteRenderer[i].color = _normalColor;
        }

        if(NotTweening()) transform.localScale = _normalScale;
    }

public void Select()
    {  
       for(int i = 0; i < _spriteRenderer.Length; i++)
       if(!DOTween.IsTweening(_spriteRenderer[i]))
       {
            //_spriteRenderer[i].DOComplete();
            _spriteRenderer[i].DOColor(_highlightedColor, _transitionDuration);
               
       }
        if(NotTweening()) transform.DOScale(_selectedScale, _transitionDuration);
 
        _selected = true;
       
    }



    public void Deselect()
    {
 
        for(int i = 0; i < _spriteRenderer.Length; i++)
        {
        _spriteRenderer[i].DOComplete();
        _spriteRenderer[i].DOColor(_normalColor, _transitionDuration);
        }
        if(NotTweening()) transform.DOScale(_normalScale, _transitionDuration);
        _selected = false;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(NotTweening())
        {
            for(int i = 0; i < _spriteRenderer.Length; i++)
            {
                _spriteRenderer[i].DOColor(_normalColor, _transitionDuration).SetLoops(2, LoopType.Yoyo);
            }
            if(NotTweening()) transform.DOScale(_normalScale, _transitionDuration).SetLoops(2, LoopType.Yoyo);
        }
    }

    private bool NotTweening()
    {
        return !DOTween.IsTweening(transform);
    }

    void FixedUpdate()
    {

        if(_multiselect)
        {
            _pointerData.position = Mouse.current.position.ReadValue();
            EventSystem.current.RaycastAll(_pointerData, _raycastResult);
            bool hit = false;
            _raycastResult.ForEach((result) => {
                if(result.gameObject == gameObject)
                {
                    hit = true;
                }
            });
            if(hit)
            {
                if(_spriteRenderer[0].color == _normalColor) Select();
            }
            else
            {
                if(_spriteRenderer[0].color == _highlightedColor) Deselect();
            }

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Deselect();
    }

}


