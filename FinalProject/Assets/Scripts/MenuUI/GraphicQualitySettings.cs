using UnityEngine;
using UnityEngine.UI;

public class GraphicQualitySettings : DropdownWhithSound 
{
    private Dropdown _dropdown;
    private string title = "QualitySettings";
    
    
    void Start()
    {
        _dropdown = GetComponent<Dropdown>();
        _dropdown.options.Clear();
        string[] qualitySettingsNames = QualitySettings.names;       
        for(int i = 0; i < QualitySettings.names.Length; i++)
        {
            _dropdown.options.Add(new Dropdown.OptionData(qualitySettingsNames[i]));
        }

        _dropdown.value = PlayerPrefs.GetInt(title, qualitySettingsNames.Length - 1);
       
    }

    public void SetGraphicQualitySettings()
    {
        QualitySettings.SetQualityLevel(_dropdown.value);
        PlayerPrefs.SetInt(title, _dropdown.value);
    }

    
}
