using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameAudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip orderUpClip;
    public AudioClip elevatorMusicClip;
    public AudioClip chewingClip;
    public AudioClip grumbleClip;
    public AudioClip readyToOrderClip;

    [Header("UI")]
    public GameObject launchScreen;
    public Text launchText;
    public KeyCode startKey = KeyCode.Space; 
    public GameObject gameplayUI;

    private AudioSource audioSource;

    private bool gameStarted = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameplayUI.SetActive(false);
        launchScreen.SetActive(true);
        launchText.text = "Welcome to Hotel Rush!\nPress SPACE to start.";
    }

    void Update()
    {
        if (!gameStarted && Input.GetKeyDown(startKey))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true;
        launchScreen.SetActive(false);
        gameplayUI.SetActive(true);
    }

    // ---------------- Sound ----------------

    public void PlayOrderUp()
    {
        audioSource.PlayOneShot(orderUpClip);
    }

    public void PlayElevatorMusic()
    {
        audioSource.clip = elevatorMusicClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopElevatorMusic()
    {
        if (audioSource.clip == elevatorMusicClip)
        {
            audioSource.Stop();
        }
    }

    public void PlayChewing()
    {
        audioSource.PlayOneShot(chewingClip);
    }

    public void PlayGrumble()
    {
        audioSource.PlayOneShot(grumbleClip);
    }

    public void PlayReadyToOrder()
    {
        audioSource.PlayOneShot(readyToOrderClip);
    }
}
