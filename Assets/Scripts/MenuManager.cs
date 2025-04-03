using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class MenuManager : MonoBehaviour
{
    private RectTransform tr;

    private GameObject pnlMainMenu, pnlLevels, pnlCredits, pnlLoadingScreen, pnlPostGame;

    private VideoPlayer vpIntro, vpOutro;

    void Awake()
    {
        tr = GetComponent<RectTransform>();
        vpIntro = tr.GetChild(0).GetComponent<VideoPlayer>();  
        pnlLoadingScreen = tr.GetChild(1).gameObject;
        pnlMainMenu = tr.GetChild(2).gameObject;
        pnlLevels = tr.GetChild(3).gameObject;
        pnlCredits = tr.GetChild(4).gameObject;
        pnlPostGame = tr.GetChild(5).gameObject;
    }

    void Start()
    {
        vpIntro.enabled = false;

        if (StaticData.toPostGame)
        {
            ToPostGame();
        }

        else
        {
            ToMainMenu();
        }
        
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
        // ToLoadingScreen();
        // rn the problem is the video is playing while the loading screen is going
        // pnlLoadingScreen.GetComponent<LoadingBar>().ToVideo("intro");
        SceneManager.LoadSceneAsync("Level 1");
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

    public void ToPostGame()
    {
        ToLoadingScreen();
        string levelName = StaticData.justPlayed;
        pnlLoadingScreen.GetComponent<LoadingBar>().ToPostGame(levelName);
    }

    public void FromLoadingToPostGame()
    {
        allInactive();
        pnlPostGame.SetActive(true);
        StaticData.toPostGame = false;
    }

    void allInactive()
    {
        pnlMainMenu.SetActive(false);
        pnlLevels.SetActive(false);
        pnlCredits.SetActive(false);
        pnlLoadingScreen.SetActive(false);
        pnlPostGame.SetActive(false);
    }
}
