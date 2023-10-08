using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider musicSlider;
    private string titleSFX = "SFX";
    private string titleMusic = "Music";
    private void Start()
    {
        GetVolume(titleSFX, SFXSlider);
        GetVolume(titleMusic, musicSlider);
    }
    public void SetSFXVolume(float sliderValue)
    {
        SetVolume(titleSFX, sliderValue);
    }
    public void SetMusicVolume(float sliderValue)
    {
        SetVolume(titleMusic, sliderValue);
    }

    public void SetVolume(string title, float value)
    {
        audioMixer.SetFloat(title, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(title, value);
    }

    public void GetVolume(string title,Slider slider)
    {
        float value = PlayerPrefs.GetFloat(title, 0.8f);
        slider.value = value;
        audioMixer.SetFloat(title, Mathf.Log10(value) * 20);
    }
}
