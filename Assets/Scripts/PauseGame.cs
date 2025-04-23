using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseGame : MonoBehaviour
{
    // for storing the reference to the scene that calls the pause
    private GameManager gm;
    private string sceneName;

    // private int selected;
    private GameObject selected, lastSelected;

    private GameObject btnResume, btnRestart, btnOptions, btnQuit;

    private GameObject resumeCloche, restartCloche, optionsCloche, quitCloche;

    private bool showingOptions;

    private GameObject quitPopUp;
    private bool showingQuitConf;

    private GameObject yesQuit, noQuit;

    // public UISoundManager uiSoundManager;

    void Awake() 
    {
        // uiSoundManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<UISoundManager>();
    }

    public void Start()
    {
        lastSelected = btnResume;

        // get the references to the buttons
        btnResume = transform.GetChild(1).gameObject;
        btnRestart = transform.GetChild(2).gameObject;
        btnOptions = transform.GetChild(3).gameObject;
        btnQuit = transform.GetChild(4).gameObject;

        // get the references to their cloches
        resumeCloche = btnResume.transform.GetChild(1).gameObject;
        restartCloche = btnRestart.transform.GetChild(1).gameObject;
        optionsCloche = btnOptions.transform.GetChild(1).gameObject;
        quitCloche = btnQuit.transform.GetChild(1).gameObject;

        // start with Start Game selected
        AllSelectionsFalse();
        resumeCloche.SetActive(true);
        EventSystem.current.SetSelectedGameObject(btnResume);

        showingOptions = false;

        Debug.Log("Paused");

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
        selected = EventSystem.current.currentSelectedGameObject;

        // to catch edge cases where mouse deselects all options
        // reset the selected option to be the last known selection
        if (selected == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }

        else if (!(selected == btnResume || selected == btnRestart || selected == btnOptions || selected == btnQuit) || !lastSelected)
        {
            EventSystem.current.SetSelectedGameObject(btnResume);
        }

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
                // uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);

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
                // uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);

                Back();
            }
        }

        if (!showingQuitConf && !showingOptions)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                // uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);

                if (selected == btnResume)
                {
                    Debug.Log("Resume game");
                    // gm.Unpause();
                    Time.timeScale = 1f;
                    Destroy(gameObject);
                }

                else if (selected == btnRestart)
                {
                    // restart confirmation pop up
                    // .GetComponent<ConfirmAction>().RestartLevel();
                    // maybe make it a bool instead of void then
                    // set a bool = RestartLevel() then act based off that
                    SceneManager.LoadSceneAsync(sceneName);
                }

                else if (selected == btnOptions)
                {
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

        if (showingOptions && Input.GetKeyDown(KeyCode.Z))
        {
            // uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);
            
            int n = SceneManager.sceneCount;

            if (n > 1)
            {
                SceneManager.UnloadSceneAsync("Controls");
            }

            showingOptions = false;
        }

        lastSelected = selected;
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

    public void GetBaseInfo(GameManager gm, string sceneName)
    {
        this.gm = gm;
        this.sceneName = sceneName;
    }
}
