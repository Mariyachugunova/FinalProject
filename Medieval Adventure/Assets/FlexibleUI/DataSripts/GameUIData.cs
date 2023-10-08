using UnityEngine;

[CreateAssetMenu(menuName = "Game UI Data")]
public class GameUIData : ScriptableObject
{
    public Color normalColor;
    public Color highlightedColor;
    public Color disabledColor;
    public float scaleFactor = 1.1f;
    public float transitionDuration = 0.2f;

}