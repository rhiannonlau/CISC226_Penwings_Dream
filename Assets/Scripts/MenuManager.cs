using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class MenuManager : MonoBehaviour
{
    private RectTransform tr;

    private GameObject pnlMainMenu, pnlLevels, pnlCredits, pnlLoadingScreen, pnlPostGame;

    private VideoPlayer vpIntro, vpOutro;

    private bool watchingVideo; // in the future, add a hasWatchedVideo? to force watch on first viewing?

    public UISoundManager uiSoundManager;

    void Awake()
    {
        uiSoundManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<UISoundManager>();

        vpIntro = GameObject.Find("vpIntro").GetComponent<VideoPlayer>();
        vpOutro = GameObject.Find("vpOutro").GetComponent<VideoPlayer>();

        tr = GetComponent<RectTransform>();
        // vpIntro = tr.GetChild(0).GetComponent<VideoPlayer>();
        // vpOutro = tr.GetChild(1).GetComponent<VideoPlayer>();
        pnlLoadingScreen = tr.GetChild(1).gameObject;
        pnlMainMenu = tr.GetChild(2).gameObject;
        pnlLevels = tr.GetChild(3).gameObject;
        pnlCredits = tr.GetChild(4).gameObject;
        pnlPostGame = tr.GetChild(5).gameObject;

        watchingVideo = false;
    }

    void Start()
    {
        if (StaticData.toPostGame)
        {
            ToPostGame();
        }

        else
        {
            ToMainMenu();
        }
    }

    void Update()
    {
        if (watchingVideo && Input.GetKeyDown(KeyCode.X))
        {
            // skip video - not sure if possible
        }
    }

    public void ToMainMenu()
    {
        AllInactive();
        pnlMainMenu.SetActive(true);
    }

    public void ToLevels()
    {
        AllInactive();
        pnlLevels.SetActive(true);
    }

    public void ToCredits()
    {
        AllInactive();
        pnlCredits.SetActive(true);
    }

    public void ToLoadingScreen()
    {
        AllInactive();
        pnlLoadingScreen.SetActive(true);
    }

    public void EndReached(VideoPlayer vp)
    {
        // StartCoroutine(PauseAfterPlay(vp));
        if (vp == vpIntro)
        {
            vpIntro.enabled = false;
            watchingVideo = false;

            ToLevel("Level 1");
        }

        else if (vp == vpOutro)
        {
            vpOutro.enabled = false;
            watchingVideo = false;

            ToCredits();
        }
    }

    public void ToLevel(string level)
    {
        ToLoadingScreen();
        pnlLoadingScreen.GetComponent<LoadingBar>().ToScene(level);
    }

    // called from loading to notify the manager that the loading screen has finished
    public void FromLoadingToLevel(string level)
    {
        SceneManager.LoadSceneAsync(level);
    }

    public void ToVideo(string video)
    {
        ToLoadingScreen();
        pnlLoadingScreen.GetComponent<LoadingBar>().ToVideo(video);
    }

    // called from loading to notify the manager that the loading screen has finished
    public void FromLoadingToVideo(string video)
    {
        AllInactive();

        uiSoundManager.StopMusic();
        
        if (video == "intro")
        {
            vpIntro.Play();

            watchingVideo = true;

            vpIntro.loopPointReached += EndReached;
        }

        else if (video == "outro")
        {
            vpOutro.Play();

            watchingVideo = true;

            vpOutro.loopPointReached += EndReached;

            uiSoundManager.PlayMusic();
        }

    }

    public void ToPostGame()
    {
        ToLoadingScreen();
        string levelName = StaticData.justPlayed;
        pnlLoadingScreen.GetComponent<LoadingBar>().ToPostGame(levelName);
    }

    public void FromLoadingToPostGame()
    {
        AllInactive();
        pnlPostGame.SetActive(true);
        StaticData.toPostGame = false;
    }

    void AllInactive()
    {
        pnlMainMenu.SetActive(false);
        pnlLevels.SetActive(false);
        pnlCredits.SetActive(false);
        pnlLoadingScreen.SetActive(false);
        pnlPostGame.SetActive(false);
    }

    IEnumerator PauseAfterPlay(VideoPlayer vp)
    {
        yield return new WaitForSeconds(2);

        if (vp == vpIntro)
        {
            vpIntro.enabled = false;
            ToLevel("Level 1");
        }

        else if (vp == vpOutro)
        {
            vpOutro.enabled = false;
            ToCredits();
        }
    }
}
