using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource musicSource;
    public AudioSource soundEffectsSource;

    public AudioClip inGameMusic;
    public AudioClip readyToOrderSound;
    public AudioClip orderReadySound;
    public AudioClip eatingSound;
    public AudioClip lowPatienceSound;
    public AudioClip endOfDaySound;
    public AudioClip moneySound;

    public AudioClip menuSelect;
    public AudioClip menuMusic;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = inGameMusic;

        // set the volume settings in player prefs with the defaults from staticdata
        musicSource.volume = PlayerPrefs.GetFloat("VolumeMusic", StaticData.musicVolume) * PlayerPrefs.GetFloat("VolumeMaster", StaticData.masterVolume);
        soundEffectsSource.volume = PlayerPrefs.GetFloat("VolumeEffects", StaticData.effectsVolume) * PlayerPrefs.GetFloat("VolumeMaster", StaticData.masterVolume);

        musicSource.Play();
    }

    public void PlaySoundEffect(AudioClip soundEffect)
    {
        soundEffectsSource.PlayOneShot(soundEffect); 
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void PlayMenuMusic()
    {
        musicSource.Play();
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }
}
