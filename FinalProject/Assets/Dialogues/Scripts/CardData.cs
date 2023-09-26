using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Data")]
public class CardData : SerializedScriptableObject
{

    [SerializeField] int _id;
    [PreviewField]
    [SerializeField] private Sprite _backgroundImage;
    [TextArea(4, 10)]
    [SerializeField] private string _text;
    [PreviewField]
    [SerializeField] private Sprite _illustrationImage;
    [SerializeField] private Dictionary<string, DialogueData> _dialogs;
    [SerializeField] private bool _answer;


    public int Id => _id;
    public string TextContent => _text;
    public Sprite BackgroundImage => _backgroundImage;
    public Sprite IllustrationImage => _illustrationImage;
    public bool Answer => _answer;

    public Dictionary<string, DialogueData> Dialogs => _dialogs;
}