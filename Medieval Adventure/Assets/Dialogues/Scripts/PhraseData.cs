using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Phrase Data")]
public class PhraseData : ScriptableObject
{
    [SerializeField] private string _characterName;    
    [PreviewField]
    [SerializeField] private Sprite _illustrationImage;
    [TextArea(4,10)]
    [SerializeField] private string _phrase;
    [SerializeField] private bool _mental;
    [SerializeField] private bool _addSelection;
    [SerializeField] private DialogueData _dialogFork;
    [SerializeField] private string _reaction;
    [SerializeField] private CardData _cardData;
    public string CharacterName => _characterName;
    public Sprite Illustration => _illustrationImage;
    public string Phrase => _phrase;
    public bool Mental => _mental;
    public bool AddSelection => _addSelection;
    public DialogueData DialogFork => _dialogFork;
    public string Reaction => _reaction;
    public CardData Card => _cardData;
}
