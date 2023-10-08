using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraTurn : MonoBehaviour
{
    CinemachineTransposer _transposer;
    private MovementController _movementController;
    private float _followOffset;
    private float _turnTime = 3;
    [Inject]
    private void Construct(MovementController movementController)
    {
        _movementController = movementController;

    }
    void Start()
    {
        _transposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
        _movementController.playerTurn += TurnCamera;
        _followOffset = _transposer.m_FollowOffset.x;
    }
    private void OnDestroy()
    {
        _movementController.playerTurn -= TurnCamera;
    }

    private void TurnCamera(float sign)
    {
        DOTween.To(FollowPffset, _transposer.m_FollowOffset.x, sign * _followOffset, _turnTime);
    }
    public void FollowPffset(float x)
    {
        _transposer.m_FollowOffset.x = x;
    }

}
