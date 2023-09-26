using UnityEngine;
using UnityEngine.UI;

public class ResolutionSetting : DropdownWhithSound
{
    private Dropdown _dropdown;
    private Resolution[] _resolutionsList;
    private int _resolutionsList_id;
    string title = "Resolution";
    void Start()
    {
        _dropdown = GetComponent<Dropdown>();
        _resolutionsList = Screen.resolutions;
        _dropdown.options.Clear();
        for(int i = 0; i < _resolutionsList.Length; i++)
        {
            _dropdown.options.Add(new Dropdown.OptionData(_resolutionsList[i].ToString()));
        }
        _dropdown.value = PlayerPrefs.GetInt(title, _resolutionsList.Length - 1);
         
    }
    public void ApplyResolution()
    {
        _resolutionsList_id = _dropdown.value;
        Screen.SetResolution(_resolutionsList[_resolutionsList_id].width, _resolutionsList[_resolutionsList_id].height, true);
        PlayerPrefs.SetInt(title, _dropdown.value);
    }
   

    
}
