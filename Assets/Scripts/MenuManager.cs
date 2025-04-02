using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class MenuManager : MonoBehaviour
{
    private RectTransform tr;

    private GameObject pnlMainMenu, pnlLevels, pnlCredits, pnlLoadingScreen;

    private VideoPlayer vpIntro, vpOutro;

    void Awake()
    {
        tr = GetComponent<RectTransform>();
        vpIntro = tr.GetChild(0).GetComponent<VideoPlayer>();  
    }

    void Start()
    {
        pnlMainMenu = tr.GetChild(2).gameObject;
        pnlLevels = tr.GetChild(3).gameObject;
        pnlCredits = tr.GetChild(4).gameObject;
        pnlLoadingScreen = tr.GetChild(1).gameObject;
        // vpOutro = tr.GetChild(2).gameObject;


        ToMainMenu();
        vpIntro.enabled = false;
    }

    public void ToMainMenu()
    {
        allInactive();
        pnlMainMenu.SetActive(true);
    }

    public void ToLevels()
    {
        allInactive();
        pnlLevels.SetActive(true);
    }

    public void ToCredits()
    {
        allInactive();
        pnlCredits.SetActive(true);
    }

    public void ToLoadingScreen()
    {
        allInactive();
        pnlLoadingScreen.SetActive(true);
    }

    // public void DisableLoadingScreen(string scene)
    // {
    //     loadingScreen.SetActive(false);

    //     if (scene != "")
    //     {

    //     }
    // }

    public void StartGame()
    {
        ToLoadingScreen();
        // rn the problem is the video is playing while the loading screen is going
        pnlLoadingScreen.GetComponent<LoadingBar>().ToVideo("intro");
    }

    public void EndReached(VideoPlayer vp)
    {
        if (vp == vpIntro)
        {
            vpIntro.enabled = false;
            ToLevel("Level 1");
        }

        else if (vp == vpIntro)
        {
            // credits

            ToMainMenu();
            // outro.enabled = false;
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

    // called from loading to notify the manager that the loading screen has finished
    public void ToVideo(string video)
    {
        if (video == "intro")
        {
            // Debug.Log(vpIntro);
            vpIntro.enabled = true;
            vpIntro.Play();
            vpIntro.loopPointReached += EndReached;
        }

        // else if (video == "outro")
        // {
        //     outro.enabled = true;
        //     outro.Play();
        //     outro.loopPointReached += EndReached;
        // }
    }

    void allInactive()
    {
        pnlMainMenu.SetActive(false);
        pnlLevels.SetActive(false);
        pnlCredits.SetActive(false);
        pnlLoadingScreen.SetActive(false);
    }
}
