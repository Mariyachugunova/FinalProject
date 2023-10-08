using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : SerializedMonoBehaviour
{
    [SerializeField] public Dictionary<string, PlayableDirector> cutsceneDataBase;

 
    public void StartCutscene(string cutsceneKey)
    {
        if (!cutsceneDataBase.ContainsKey(cutsceneKey)) 
        {
            Debug.LogError($"�������� c ������ \"{cutsceneKey}\" ���� � cutsceneDataBase");
            return;
        } 

        // ���� ������ ������������� �������� � �� �������� ������� � ���� ������ Ũ �� �� ������ �������� ���������� ������
        
        cutsceneDataBase[cutsceneKey].Play();
    }
  
}
