using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform _dialogues;
    public Dialogue CurrentDialog { get { return _dialogues.GetChild(0).GetComponent<Dialogue>(); } }
    public Transform DialogPosition { get; set; }
    private void Start()
    {
        gameObject.SetActive(false);       
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        for(int i = 0; i < _dialogues.childCount; i++)
        {
             GameObject dialog = _dialogues.GetChild(i).gameObject;
             if (dialog.activeSelf) dialog.GetComponent<Dialogue>().Hide();
        }

    }
    public void ClearExtraDialogs()
    {
        for(int i = 1; i < _dialogues.childCount; i++)
        {
            Destroy(_dialogues.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        if (gameObject.activeSelf) _dialogues.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, DialogPosition.transform.position);
    }
}
