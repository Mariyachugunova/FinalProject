using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class DialogPoint : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PathPoint _pathPoint;
    [SerializeField] private DialogueData _dialogueData;
    private MovementController _movementController;
    public PathPoint CharacterPathPoint => _pathPoint;
    private DialoguePanel _dialoguePanel;
    [Inject]
    private void Construct(DialoguePanel dialoguePanel, MovementController movementController)
    {
        _dialoguePanel = dialoguePanel;
        _movementController = movementController;
 
    }
    private void Start()
    {
        _movementController.ReachPlace += StopAndTurn;
    }
    private void OnDestroy()
    {
        _movementController.ReachPlace -= StopAndTurn;
 

    }

    private Path _path;
    [Inject]
    private void Construct(Path path)
    {
        _path = path;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!_pathPoint._inPlase)
        {
            _path.GoToPoint(_pathPoint);
        }         
    }

    private void StopAndTurn()
    {
        if(_pathPoint._inPlase)
        {
            _movementController.FaceToX(transform.position.x);
            _dialoguePanel.gameObject.SetActive(true);
            _dialoguePanel.CurrentDialog.Activate(_dialogueData);

        }
    }

}

