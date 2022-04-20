using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource audioSource;

    public bool sound = true;

    private void Awake()
    {
        MakeSingleton();
        audioSource = GetComponent<AudioSource>();
    }

    void MakeSingleton()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SoundOnOff()
    {
        sound = !sound;
    }

    public void VibrationOnOff()
    {
        Vibration.vibrationOn = !Vibration.vibrationOn;
    }

    public void PlaySoundFX(AudioClip clip, float volume)
    {
        if (sound)
            audioSource.PlayOneShot(clip,volume);
    }
}
