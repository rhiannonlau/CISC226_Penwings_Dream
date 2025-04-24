using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource soundEffectsSource;

    public AudioClip menuMusic;
    public AudioClip menuSelectSound;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = menuMusic;
        UpdateMasterVolume();
        musicSource.Play();
    }
    
    public void PlaySoundEffect(AudioClip soundEffect)
    {
        soundEffectsSource.PlayOneShot(soundEffect);
    }

    // to pause and play the music when the videos are playing
    public void PlayMusic()
    {
        musicSource.Play();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void UpdateMusicVolume()
    {
        musicSource.volume = StaticData.musicVolume * StaticData.masterVolume;
    }

    public void UpdateEffectsVolume()
    {
        soundEffectsSource.volume = StaticData.effectsVolume * StaticData.masterVolume;
    }

    public void UpdateMasterVolume()
    {
        UpdateMusicVolume();
        UpdateEffectsVolume();
    }
}
