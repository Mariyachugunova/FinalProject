using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
    public enum Direction
    {
        left,
        up,        
        right,
        down
    }
public class PathPoint : SerializedMonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] public bool _inPlase;// { get; set; }
    private MovementController _movementController;
    [Inject]
    private void Construct(MovementController movementController)
    {
        _movementController = movementController;
    }


    public Dictionary<Direction, PathPoint> _derections = new();


    public void OnPointerClick(PointerEventData eventData)
    {
    
        //_movementController.GoToPathPoint(this);

    }

}
