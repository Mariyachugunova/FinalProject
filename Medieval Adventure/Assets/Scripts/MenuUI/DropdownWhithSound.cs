using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class DropdownWhithSound : MonoBehaviour, ISelectHandler
{
    protected ButtonSFX _buttonSFX;

    [Inject]
    private void Construct(GlobalObjects go)
    {
        _buttonSFX = go.ButtonSFX;

    }
    public void OnSelect(BaseEventData eventData)
    {
        if(transform.childCount == 3) _buttonSFX.PlayOnHoverSound();
    }


     
}
