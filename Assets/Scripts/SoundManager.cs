using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = inGameMusic;
        musicSource.Play();
    }

    public void PlaySoundEffect(AudioClip soundEffect)
    {
        soundEffectsSource.PlayOneShot(soundEffect); 
    }

}
