using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class ButtonSelect : MonoBehaviour, IPointerEnterHandler,ISelectHandler
{

    private ButtonSFX _buttonSFX;
    private Button _button;

    [Inject]
    private void Construct(GlobalObjects go)
    {
        _buttonSFX = go.ButtonSFX;

    }

    private void Start()
    {
        _button = GetComponent<Button>();
 
    }
    public void PlayOnhoverSound() {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _button.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        _buttonSFX.PlayOnHoverSound();
        
    }
}
