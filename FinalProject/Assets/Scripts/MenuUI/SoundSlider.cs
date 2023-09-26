using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour,IPointerEnterHandler,ISelectHandler
{
   
    [SerializeField] private string _title;
    private Slider _slider; 
    [SerializeField] private AudioMixer _audioMixer;
    private void Start()
    {
        _slider = GetComponent<Slider>();
        float value = PlayerPrefs.GetFloat(_title, 0.8f);
        _slider.value = value;
        _audioMixer.SetFloat(_title, Mathf.Log10(value) * 20);
        if(_title == "SFX") _slider.onValueChanged.AddListener(OnValueChange);
    }

    public void SetVolume( float value)
    {
        _audioMixer.SetFloat(_title, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(_title, value);
    }
    public void OnSelect(BaseEventData eventData)
    {
        EventManager.PlayButtonSound_Invoke();
    }
    public void OnValueChange(float value)
    {
        EventManager.PlayButtonSound_Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _slider.Select();
    }
}
