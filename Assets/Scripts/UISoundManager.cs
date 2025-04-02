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
        musicSource.Play();
    }
    
    public void PlaySoundEffect(AudioClip soundEffect)
    {
        soundEffectsSource.PlayOneShot(soundEffect); 
    }

}
