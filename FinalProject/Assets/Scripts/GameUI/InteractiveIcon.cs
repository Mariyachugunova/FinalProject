using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InteractiveIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{

    [SerializeField] protected GameUIData _UISkin;
    [SerializeField] protected SpriteRenderer _sprite;
    [SerializeField] protected Transform _transform;
    private bool isSelected;
    public bool IsSelected => isSelected;

    private static InteractiveIcon SelectedObject;

    private TextMeshProUGUI _text;
    private Color _normalColor;
    private Color _highlightedColor;

    private Vector3 _normalScale;
    private Vector3 _selectedScale;
    private float _transitionDuration = 0.2f;

    private void Start()
    {
        _normalColor = _UISkin._normalColor;
        _highlightedColor = _UISkin._highlightedColor;
        _transitionDuration = _UISkin._transitionDuration;
        _sprite.color = _normalColor;
        _normalScale = Vector3.one* _UISkin._normalScale;
        _selectedScale = Vector3.one * _UISkin._selectedScale;
        _text = GetComponentInChildren<TextMeshProUGUI>();
        SetDefaultState();
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Select();
        if(SelectedObject != null && SelectedObject != this)
        {
            SelectedObject.Deselect();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (SelectedObject != this) Deselect();

        if(SelectedObject != null && SelectedObject != this)
        {
            SelectedObject.Select();
        }
    }
    public void SetDefaultState()
    {
        _sprite.color = _normalColor;
        _transform.localScale = _normalScale;
        _text?.gameObject.SetActive(false);
    }
  

    public void Select()
    {
        _sprite.DOColor(_highlightedColor, _transitionDuration);
        _transform.DOScale(_selectedScale, _transitionDuration);
        _text?.gameObject.SetActive(true);
        isSelected = true;
        
    }

    public void Deselect()
    {
        _sprite.DOColor(_normalColor, _transitionDuration);
        _transform.DOScale(_normalScale, _transitionDuration).OnComplete(()=> isSelected = false);
        _text?.gameObject.SetActive(false);
       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Deselect();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Select();
            SelectedObject = this;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player") Deselect();
        SelectedObject = null;
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
