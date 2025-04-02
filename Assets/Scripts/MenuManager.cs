using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu, levels, credits, loadingScreen;

    [SerializeField] VideoPlayer intro, outro;

    void Start()
    {
        ToMainMenu(); 
    }

    public void ToMainMenu()
    {
        allInactive();
        mainMenu.SetActive(true);
    }

    public void ToLevels()
    {
        allInactive();
        levels.SetActive(true);
    }

    public void ToCredits()
    {
        allInactive();
        credits.SetActive(true);
    }

    public void ToLoadingScreen()
    {
        allInactive();
        loadingScreen.SetActive(true);
    }

    public void StartGame()
    {
        ToLoadingScreen();
        // rn the problem is the video is playing while the loading screen is going
        intro.enabled = true;
        intro.Play();
        intro.loopPointReached += EndReached;
    }

    public void EndReached(VideoPlayer vp)
    {
        if (vp == intro)
        {
            intro.enabled = false;
            ToLevel("Level 1");
        }

        else if (vp == outro)
        {
            ToMainMenu();
            // credits
        }
    }

    public void ToLevel(string level)
    {
        ToLoadingScreen();
        loadingScreen.GetComponent<LoadingBar>().ToScene(level);
    }

    void allInactive()
    {
        mainMenu.SetActive(false);
        levels.SetActive(false);
        credits.SetActive(false);
        loadingScreen.SetActive(false);
    }
}
