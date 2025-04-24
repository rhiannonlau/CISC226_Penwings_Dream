using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseGame : MonoBehaviour
{
    // for storing the reference to the scene that calls the pause
    private GameManager gm;
    private string sceneName;
    Scene pauseScene;

    // private int selected;
    private GameObject selected, lastSelected;

    private GameObject btnResume, btnRestart, btnOptions, btnQuit;

    private GameObject resumeCloche, restartCloche, optionsCloche, quitCloche;

    private bool showingOptions;

    private GameObject quitPopUp;
    private bool showingQuitConf;

    private GameObject yesQuit, noQuit;

    public UISoundManager uiSoundManager;

    private EventSystem eventSystem;

    void Awake() 
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        uiSoundManager = GameObject.Find("UI Sound Manager").GetComponent<UISoundManager>();
        pauseScene = SceneManager.GetSceneByName("PauseScreen");
    }

    public void Start()
    {
        // get the references to the buttons
        btnResume = transform.GetChild(1).gameObject;
        btnRestart = transform.GetChild(2).gameObject;
        btnOptions = transform.GetChild(3).gameObject;
        btnQuit = transform.GetChild(4).gameObject;

        lastSelected = btnResume;

        // get the references to their cloches
        resumeCloche = btnResume.transform.GetChild(1).gameObject;
        restartCloche = btnRestart.transform.GetChild(1).gameObject;
        optionsCloche = btnOptions.transform.GetChild(1).gameObject;
        quitCloche = btnQuit.transform.GetChild(1).gameObject;

        // start with Start Game selected
        AllSelectionsFalse();
        resumeCloche.SetActive(true);
        eventSystem.SetSelectedGameObject(btnResume);

        showingOptions = false;

        sceneName = StaticData.currentLevel;

        // quit pop up hidden
        // quitPopUp = transform.GetChild(6).gameObject;
        // showingQuitConf = false;

        // // get the yes and no options for the quit confirmation
        // yesQuit = quitPopUp.transform.GetChild(1).gameObject;
        // noQuit = quitPopUp.transform.GetChild(2).gameObject;
    }

    public void Update()
    {
        if (SceneManager.sceneCount == 2 && !eventSystem.enabled)
        {
            eventSystem.enabled = true;
            showingOptions = false;
        }

        // to catch edge cases where mouse deselects all options
        // reset the selected option to be the last known selection
        if (!selected && (lastSelected == btnResume || lastSelected == btnRestart || lastSelected == btnOptions || lastSelected == btnQuit))
        {
            eventSystem.SetSelectedGameObject(lastSelected);
        }

        else if (!(selected == btnResume || selected == btnRestart || selected == btnOptions || selected == btnQuit) && !showingOptions)
        {
            eventSystem.SetSelectedGameObject(btnResume);
            lastSelected = btnResume;
        }

        selected = eventSystem.currentSelectedGameObject;

        // quitPopUp.SetActive(showingQuitConf);

        // depending on which option is selected, show the cloche
        if (selected == btnResume)
        {
            AllSelectionsFalse();
            resumeCloche.SetActive(true);
        }

        else if(selected == btnRestart)
        {
            AllSelectionsFalse();
            restartCloche.SetActive(true);
        }

        else if(selected == btnOptions)
        {
            AllSelectionsFalse();
            optionsCloche.SetActive(true);
        }

        else if (selected == btnQuit)
        {
            AllSelectionsFalse();
            quitCloche.SetActive(true);
        }

        else
        {
            AllSelectionsFalse();
        }

        // if the quit confirmation is showing
        if (showingQuitConf && !showingOptions)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);

                if (selected == yesQuit)
                {
                    SceneManager.LoadSceneAsync("Menus");
                }

                else if (selected == noQuit)
                {
                    Back();
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);

                Back();
            }
        }

        if (!showingQuitConf && !showingOptions)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);

                if (selected == btnResume)
                {
                    Debug.Log("Resume game");
                    // gm.Unpause();
                    Time.timeScale = 1f;

                    int n = SceneManager.sceneCount;

                    if (n > 1)
                    {
                        SceneManager.UnloadSceneAsync("PauseScreen");
                    }

                    uiSoundManager.PauseMusic();
                }

                else if (selected == btnRestart)
                {
                    // restart confirmation pop up
                    // .GetComponent<ConfirmAction>().RestartLevel();
                    // maybe make it a bool instead of void then
                    // set a bool = RestartLevel() then act based off that
                    SceneManager.LoadSceneAsync(sceneName);
                    uiSoundManager.PauseMusic();
                }

                else if (selected == btnOptions)
                {
                    eventSystem.enabled = false;// turn off this eventsystem to prevent conflicts with the eventsystem in options
                    SceneManager.LoadScene("Options", LoadSceneMode.Additive);
                    showingOptions = true;
                }

                else if (selected == btnQuit)
                {
                    // showingQuitConf = true;
                    // EventSystem.current.SetSelectedGameObject(noQuit);
                    SceneManager.LoadSceneAsync("Menus");
                }
            }
        }

        lastSelected = selected;
    }

    private void Back()
    {
        showingQuitConf = false;
        eventSystem.SetSelectedGameObject(btnResume);
    }

    // helper function to turn off all cloches
    private void AllSelectionsFalse()
    {
        resumeCloche.SetActive(false);
        restartCloche.SetActive(false);
        optionsCloche.SetActive(false);
        quitCloche.SetActive(false);
    }

    public void GetBaseInfo(GameManager gm, string sceneName)
    {
        this.gm = gm;
        this.sceneName = sceneName;
    }
}
