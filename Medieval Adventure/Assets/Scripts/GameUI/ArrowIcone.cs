using ModestTree.Util;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ArrowIcone : MonoBehaviour,IPointerClickHandler
{
    private Transform _target;
    private Direction _direction { get; set; }

    private MovementController _playerMovementController;

    public void OnPointerClick(PointerEventData eventData)
    {
      EventManager.ArrowClick_Invoke(_direction);
    }

    public void Initialize(Transform nextPointransform,string spriteName, Direction direction)
    {
        SetLookRotation(nextPointransform);
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spriteName);
        _direction = direction;

    }
    
    public float _angle;
    public void SetLookRotation(Transform target)
    {
        _target = target;
        Vector3 dir = _target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        float spritewith = transform.GetComponent<SpriteRenderer>().sprite.rect.width*2;
        transform.position = _target.position ;
        gameObject.SetActive(false);
        _angle  = angle;
        if(Math.Abs(angle) > 90)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
    }
 
}
