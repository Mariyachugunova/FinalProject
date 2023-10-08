using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class DialogPoint : MonoBehaviour, IPointerClickHandler,IRunable
{
    [SerializeField] private PathPoint _pathPoint;
    [SerializeField] private DialogueData _dialogueData;
    private MovementController _movementController;
    [SerializeField] private InteractiveIcon _icone;
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
        _movementController.ReachPlace += TurnAndTalk;
    }
    private void OnDestroy()
    {
        _movementController.ReachPlace -= TurnAndTalk;
 

    }

    private Path _path;
    [Inject]
    private void Construct(Path path)
    {
        _path = path;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DialogActivate();
    }

    private void DialogActivate()
    {
        if(!_pathPoint._inPlase)
        {
            _path.GoToPoint(_pathPoint);
        }
        else
        {
            _dialoguePanel.CurrentDialog.Activate(_dialogueData);

        }
    }

    private void TurnAndTalk()
    {
        if(_pathPoint._inPlase)
        {
            _movementController.FaceToX(transform.position.x);
            _dialoguePanel.CurrentDialog.Activate(_dialogueData);

        }
    }

    public void Run()
    {
        if (_icone.IsSelected && !_dialoguePanel.gameObject.activeSelf) DialogActivate();
    }
}

