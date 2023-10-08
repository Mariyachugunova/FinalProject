using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractiveGUIElement : SerializedMonoBehaviour, IPointerClickHandler 
{

    [SerializeField] protected GameUIData _UISkin;
    Image[] _images;
    private Color _normalColor;
    private Color _highlightedColor;
    private Vector3 _normalScale;
    private Vector3 _selectedScale;
    private float _transitionDuration;
    private PointerEventData _pointerData;
    private List<RaycastResult> _raycastResult;
    public static InteractiveGUIElement _selectedElement;
    private void Awake()
    {
        _images = GetComponentsInChildren<Image>();
        _normalColor = _UISkin._normalColor;
        _highlightedColor = _UISkin._highlightedColor;     
        _transitionDuration = _UISkin._transitionDuration;
        _normalScale = Vector3.one* _UISkin._normalScale;
        _selectedScale = Vector3.one* _UISkin._selectedScale;
        _pointerData = new(EventSystem.current);
        _raycastResult = new();
    }
    public void SetDefaultState()
    {
        for(int i = 0; i < _images.Length; i++)
        {
             _images[i].color = _normalColor;
        }

        if(NotTweening()) transform.localScale = _normalScale;
        if(_selectedElement == this) _selectedElement = null;
    }
    public void SetSelectedState()
    {
        for(int i = 0; i < _images.Length; i++)
        {
            _images[i].color = _highlightedColor;
        }

        if(NotTweening()) transform.localScale = _selectedScale;
        _selectedElement = this;
    }


    public void Select()
    {
        if(_selectedElement != this)
        {
            _selectedElement?.Deselect();
            for(int i = 0; i < _images.Length; i++)
            {
                float a = _images[i].color.a;
                Color newColor = new(_highlightedColor.r, _highlightedColor.g, _highlightedColor.b, a);

                _images[i].DOColor(newColor, _transitionDuration);

            }
            if(NotTweening()) transform.DOScale(_selectedScale, _transitionDuration);

            _selectedElement = this;
        }
    }

    public void Deselect()
    {
        if(_selectedElement == this) _selectedElement = null;
        for(int i = 0; i < _images.Length; i++)
        {
            //_images[i].DOComplete();
            float a = _images[i].color.a;
            Color newColor = new(_normalColor.r, _normalColor.g, _normalColor.b, a);
            _images[i].DOColor(newColor, _transitionDuration);
        }
        if(NotTweening()) transform.DOScale(_normalScale, _transitionDuration);

    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if(NotTweening())
        {
            for(int i = 0; i < _images.Length; i++)
            {
                _images[i].DOColor(_normalColor, _transitionDuration).SetLoops(2, LoopType.Yoyo);
            }
            if(NotTweening()) transform.DOScale(_normalScale, _transitionDuration).SetLoops(2, LoopType.Yoyo);
        }
    }

    private bool NotTweening()
    {
        return !DOTween.IsTweening(transform);
    }
    public void RayCast()
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
            SetSelectedState();
        }
        else
        {
            SetDefaultState();
        }
    }

   
}


