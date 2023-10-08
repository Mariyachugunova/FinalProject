using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

public class Character : MonoBehaviour
{
    [SerializeField] private string _characterName;
    public bool _isAnimationPlaing;
    public string CharacterName => _characterName;
    
}
