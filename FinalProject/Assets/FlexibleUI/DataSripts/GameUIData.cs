using UnityEngine;

[CreateAssetMenu(menuName = "Game UI Data")]
public class GameUIData : ScriptableObject
{
    public Color _normalColor;
    public Color _highlightedColor;
    public Color _disabledColor;
    public float _normalScale;
    public float _selectedScale;
    public float _transitionDuration;

}