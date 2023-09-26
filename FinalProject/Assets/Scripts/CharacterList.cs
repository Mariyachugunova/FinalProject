using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<string, GameObject> _characterList = new();
    private List<string> _characterNames = new();
    public event Action EndOfAnimation;
    public Dictionary<string, GameObject> CharactersList => _characterList;
    private void Start()
    {
        _characterList.Clear();
        foreach(var item in FindObjectsOfType<Character>())
        {
            _characterList.Add(item.CharacterName,item.gameObject);
        }
        
        
    }

    //Послать параметр в аниматор каждому учаснику диалога
    internal void PlayAnimation(string parametr, DialogueData dialogueData)
    {
        _characterNames.Clear();
        foreach(var item in dialogueData.phrases)
        {
            if(!_characterNames.Contains(item.CharacterName))
            {
                Animator animator = _characterList[item.CharacterName].GetComponentInChildren<Animator>();
                animator?.SetTrigger(parametr);
                _characterNames.Add(item.CharacterName);
            }
        }    
    }

    public void EndAnimationCheck(string characterName)
    {
        if(_characterNames.Count > 0)
        {
            _characterNames.Remove(characterName);
            if(_characterNames.Count == 0) EndOfAnimation?.Invoke();
        }
    }


}
