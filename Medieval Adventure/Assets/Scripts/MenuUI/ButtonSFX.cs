using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonSFX : MonoBehaviour
{
    private new AudioSource  audio;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        EventManager.PlayButtonSound += PlayOnHoverSound;
    }

    private void OnDestroy()
    {
        EventManager.PlayButtonSound -= PlayOnHoverSound;
    }
    public void PlayOnHoverSound()
    {
        audio.clip = (AudioClip)Resources.Load("shurshanie");
        if (!audio.isPlaying) audio.Play();
    }
    public void PlaySound(string soundName)
    {
        audio.PlayOneShot((AudioClip)Resources.Load(soundName));
    }

}
