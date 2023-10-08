using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList : MonoBehaviour
{
    private Dictionary<string, Character> _characterList = new();
    public event Action EndOfAnimation;
    public Dictionary<string, Character> CharactersList => _characterList;
   

    private void Start()
    {
        _characterList.Clear();
        foreach(var item in FindObjectsOfType<Character>())
        {
            _characterList.Add(item.CharacterName,item);
        }
        
        
    }

 


}
