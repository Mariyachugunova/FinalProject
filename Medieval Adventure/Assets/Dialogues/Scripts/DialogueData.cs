using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [SerializeField] private PhraseData[] _phrases;
    public PhraseData[] phrases => _phrases; 
     
}
