using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class PathPoint : SerializedMonoBehaviour
{
    [HideInInspector] public bool _check;
    [SerializeField] private Character _character;
    public bool _inPlase { get; set; }
    private MovementController _movementController;
    [Inject]
    private void Construct(MovementController movementController)
    {
        _movementController = movementController;
    }
    public enum Direction
    {
        left,
        up,        
        right,
        down
    }

    public Dictionary<Direction, PathPoint> _derections = new();


    public void OnPointerClick(PointerEventData eventData)
    {
    
        //_movementController.GoToPathPoint(this);

    }

}
