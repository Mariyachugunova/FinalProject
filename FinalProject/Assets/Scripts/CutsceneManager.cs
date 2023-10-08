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
            Debug.LogError($"Катсцены c ключом \"{cutsceneKey}\" нету в cutsceneDataBase");
            return;
        } 

        // Если сейчас проигрывается катсцена и мы пытаемся вызвать в этот момент ЕЁ ЖЕ то просто обрываем выполнение метода
        
        cutsceneDataBase[cutsceneKey].Play();
    }
  
}
