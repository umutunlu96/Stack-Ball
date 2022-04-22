using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    [Header("Sound Options")]
    public bool isMuted;

    [Header("Vibration Options")]
    public bool isNotVibrating;

    private void Awake()
    {
        MakeSingleton();
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

    private void Start()
    {
        isMuted = PlayerPrefs.GetInt("Muted") == 1;
        SoundManager.instance.audioSource.mute = isMuted;
        isNotVibrating = PlayerPrefs.GetInt("Vibrate", 1) == 1;
    }

    public void MuteToggle()
    {
        isMuted = !isMuted;
        SoundManager.instance.audioSource.mute = isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
    }

    public void VibrationToggle()
    {
        isNotVibrating = !isNotVibrating;
        PlayerPrefs.SetInt("Vibrate", isNotVibrating ? 1 : 0);
    }
}