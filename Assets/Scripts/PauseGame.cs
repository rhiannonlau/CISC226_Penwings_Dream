using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseGame : MonoBehaviour
{
    // private int selected;
    private GameObject selected;
    private GameObject lastSelected;

    private GameObject btnResume, btnRestart, btnOptions, btnQuit;

    private GameObject resumeCloche, restartCloche, optionsCloche, quitCloche;

    private bool showingControls;
    private bool showingOptions;

    private GameObject quitPopUp;
    private bool showingQuitConf;

    private GameObject yesQuit;
    private GameObject noQuit;

    public UISoundManager uiSoundManager;

    void Awake() 
    {
        uiSoundManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<UISoundManager>();
    }

    public void Start()
    {
        lastSelected = btnResume;

        // get the references to the buttons
        btnResume = transform.GetChild(2).gameObject;
        btnRestart = transform.GetChild(2).gameObject;
        btnOptions = transform.GetChild(3).gameObject;
        btnQuit = transform.GetChild(4).gameObject;

        // get the references to their cloches
        resumeCloche = btnResume.transform.GetChild(1).gameObject;
        optionsCloche = btnOptions.transform.GetChild(1).gameObject;
        quitCloche = btnQuit.transform.GetChild(1).gameObject;

        // start with Start Game selected
        AllSelectionsFalse();
        resumeCloche.SetActive(true);
        EventSystem.current.SetSelectedGameObject(btnResume);

        showingControls = false;

        // quit pop up hidden
        quitPopUp = transform.GetChild(6).gameObject;
        showingQuitConf = false;

        // get the yes and no options for the quit confirmation
        yesQuit = quitPopUp.transform.GetChild(1).gameObject;
        noQuit = quitPopUp.transform.GetChild(2).gameObject;
    }

    public void Update()
    {
        selected = EventSystem.current.currentSelectedGameObject;

        // to catch edge cases where mouse deselects all options
        // reset the selected option to be the last known selection
        if (selected == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }

        // else if (!(selected == btnLvl1 || selected == btnLvl2 || selected == btnLvl3 || selected == btnLvl4 || selected == btnLvl5 || selected == btnMainMenu) || !lastSelected)
        // {
        //     EventSystem.current.SetSelectedGameObject(btnLvl1);
        // }

        quitPopUp.SetActive(showingQuitConf);

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
        if (showingQuitConf && !showingControls)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);

                if (selected == yesQuit)
                {
                    MainMenu();
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

        if (!showingQuitConf && !showingControls)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);

                if (selected == btnResume)
                {
                    ResumeGame();
                }

                else if (selected == btnRestart)
                {
                    // restart confirmation pop up
                    // .GetComponent<ConfirmAction>().RestartLevel();
                    // maybe make it a bool instead of void then
                    // set a bool = RestartLevel() then act based off that
                }

                else if (selected == btnOptions)
                {
                    // ShowControls();
                }

                else if (selected == btnQuit)
                {
                    ConfirmQuit();
                }
            }
        }

        if (showingControls && Input.GetKeyDown(KeyCode.Z))
        {
            uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);
            
            HideControls();
        }

        lastSelected = selected;
    }

    // user presses resume game
    private void ResumeGame()
    {
        Debug.Log("Resume game");
        // SceneManager.LoadSceneAsync("LoadingScreen");
    }

    private void ShowControls()
    {
        SceneManager.LoadScene("Controls", LoadSceneMode.Additive);
        showingControls = true;
    }

    public void HideControls()
    {
        int n = SceneManager.sceneCount;

        if (n > 1)
        {
            SceneManager.UnloadSceneAsync("Controls");
        }

        showingControls = false;
    }

    // user presses quit game, give a quit confirmation popup
    // to verify that this is what they want
    private void ConfirmQuit()
    {
        showingQuitConf = true;
        EventSystem.current.SetSelectedGameObject(noQuit);
    }

    private void MainMenu()
    {
        SceneManager.LoadSceneAsync("StartMenu");
    }

    private void Back()
    {
        showingQuitConf = false;
        EventSystem.current.SetSelectedGameObject(btnResume);
    }

    // helper function to turn off all cloches
    private void AllSelectionsFalse()
    {
        resumeCloche.SetActive(false);
        restartCloche.SetActive(false);
        optionsCloche.SetActive(false);
        quitCloche.SetActive(false);
    }
}
