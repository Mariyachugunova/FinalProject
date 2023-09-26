using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{

    [SerializeField] protected GameUIData UISkin;
    [SerializeField] SpriteRenderer spriteRenderer;
    private Color normalColor;
    private Color highlightedColor;

    private Vector3 normalScale;
    private Vector3 selectedScale;
    private float scaleFactor = 1.1f;
    private float transitionDuration = 0.2f;
    private void Start()
    {
        normalColor = UISkin.normalColor;
        highlightedColor = UISkin.highlightedColor;
        scaleFactor = UISkin.scaleFactor;
        transitionDuration = UISkin.transitionDuration;
        spriteRenderer.color = normalColor;
        normalScale = transform.localScale;
        selectedScale = normalScale*scaleFactor;
        DeSelect();
    }

 
    public void OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DeSelect();
    }

    public void Select()
    {
        spriteRenderer.DOColor(highlightedColor, transitionDuration);
        transform.DOScale(selectedScale, transitionDuration);

    }

    public void DeSelect()
    {
        spriteRenderer.DOColor(normalColor, transitionDuration);
        transform.DOScale(normalScale, transitionDuration);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DeSelect();
    }

    public void DeselectAndHide()
    {
        
            spriteRenderer?.DOColor(normalColor, transitionDuration);
            transform.DOScale(normalScale, transitionDuration).onComplete = Hide;
        
    }
    private void Hide() {
        gameObject.SetActive(false);

    }
}
