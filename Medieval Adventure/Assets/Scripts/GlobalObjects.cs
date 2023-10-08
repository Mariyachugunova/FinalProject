using UnityEngine;
using UnityEngine.UI;

public class GlobalObjects : MonoBehaviour
{
    [SerializeField] private Image _loadingPanel;
    public Image LoadingPanel => _loadingPanel;

    [SerializeField] private ButtonSFX _buttonSFX;
    public ButtonSFX ButtonSFX => _buttonSFX;
}
