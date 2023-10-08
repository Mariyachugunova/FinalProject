using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Character : MonoBehaviour
{
    [SerializeField] private string _characterName;

    public string CharacterName => _characterName;


}
