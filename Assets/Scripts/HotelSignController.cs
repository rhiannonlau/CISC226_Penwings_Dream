using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotelSignController : MonoBehaviour
{
    [SerializeField] GameObject signOn, signOff;

    public SoundManager soundManager;

    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundManager>();

        StartCoroutine(InitialOn());
        StartCoroutine(Blink());
    }

    IEnumerator InitialOn()
    {        
        yield return new WaitForSeconds(1);

        signOn.SetActive(true);
    }

    IEnumerator Blink()
    {
        while (true)
        {
            signOn.SetActive(true);

            yield return new WaitForSeconds(Random.Range(0f, 3f));

            signOn.SetActive(false);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void TurnOff()
    {
        StartCoroutine(WaitTurnOff());
    }

    IEnumerator WaitTurnOff()
    {
        yield return new WaitForSecondsRealtime(2);

        soundManager.PlaySoundEffect(soundManager.menuSelect);
        signOn.SetActive(false);
    }
}
