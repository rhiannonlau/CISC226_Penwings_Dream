using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    // reference canvas to access MenuManager.cs to change panels
    [SerializeField] private Canvas canvas;

    // references to the selected button
    private GameObject selected, lastSelected;

    // references to the buttons
    private GameObject btnStart, btnLevels, btnTutorial, btnOptions, btnCredits, btnQuit;

    // references to the cloches for indicating which button is being selected
    private GameObject startCloche, levelsCloche, tutorialCloche, creditsCloche, optionsCloche, quitCloche;

    // true when the controls panel is showing
    private bool showingOptions;

    // controlling the quit confirmation pop up
    private GameObject quitPopUp;
    private bool showingQuitConf;
    private GameObject btnYesQuit, btnNoQuit;
    private GameObject yesQuitCloche, noQuitCloche;

    public UISoundManager uiSoundManager;

    private EventSystem eventSystem;

    void Awake() 
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        // get the references to the buttons
        btnStart = transform.GetChild(1).gameObject;
        btnLevels = transform.GetChild(2).gameObject;
        btnTutorial = transform.GetChild(3).gameObject;
        btnOptions = transform.GetChild(4).gameObject;
        btnCredits = transform.GetChild(5).gameObject;
        btnQuit = transform.GetChild(6).gameObject;

        // get the references to their cloches
        startCloche = btnStart.transform.GetChild(1).gameObject;
        levelsCloche = btnLevels.transform.GetChild(1).gameObject;
        tutorialCloche = btnTutorial.transform.GetChild(1).gameObject;
        optionsCloche = btnOptions.transform.GetChild(1).gameObject;
        creditsCloche = btnCredits.transform.GetChild(1).gameObject;
        quitCloche = btnQuit.transform.GetChild(1).gameObject;

        // get the reference to the quit confirmation pop up
        quitPopUp = transform.GetChild(7).gameObject;
        btnYesQuit = quitPopUp.transform.GetChild(1).gameObject;
        btnNoQuit = quitPopUp.transform.GetChild(2).gameObject;
        yesQuitCloche = btnYesQuit.transform.GetChild(1).gameObject;
        noQuitCloche = btnNoQuit.transform.GetChild(1).gameObject;

        // set the first selection on awake
        lastSelected = btnStart;
        // EventSystem.current.SetSelectedGameObject(btnStart);

        // uiSoundManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<UISoundManager>();
    }

    
    public void OnEnable()
    {
        AllSelectionsFalse();

        // make sure other pop ups are hidden
        showingOptions = false;
        showingQuitConf = false;

        // start with the last option that was selected
        EventSystem.current.SetSelectedGameObject(lastSelected);
    }

    public void Update()
    {
        if (SceneManager.sceneCount == 1 && !eventSystem.enabled)
        {
            eventSystem.enabled = true;
            showingOptions = false;
        }

        if (SceneManager.sceneCount == 1)
        {
            showingOptions = false;
            // showingQuitConf = false;
        }

        // get the current selection
        selected = eventSystem.currentSelectedGameObject;

        quitPopUp.SetActive(showingQuitConf);
        Debug.Log(eventSystem.currentSelectedGameObject);

        if (!showingQuitConf && !showingOptions)
        {
            // to catch edge cases where mouse deselects all options
            // reset the selected option to be the last known selection
            if (!selected && (lastSelected == btnStart || lastSelected == btnLevels || lastSelected == btnTutorial || lastSelected == btnOptions || lastSelected == btnCredits || lastSelected == btnQuit))
            {
                eventSystem.SetSelectedGameObject(lastSelected);
            }

            // if selected is not = to a button in this panel and lastSelected is null, reset to btnStart
            else if (!(selected == btnStart || selected == btnLevels || selected == btnTutorial || selected == btnOptions || selected == btnCredits || selected == btnQuit) && !showingOptions)
            {
                eventSystem.SetSelectedGameObject(btnStart);
                lastSelected = btnStart;
            }

            // depending on which option is currently being hovered, show the cloche
            if (selected == btnStart)
            {
                AllSelectionsFalse();
                startCloche.SetActive(true);
            }

            else if(selected == btnLevels)
            {
                AllSelectionsFalse();
                levelsCloche.SetActive(true);
            }

            else if(selected == btnTutorial)
            {
                AllSelectionsFalse();
                tutorialCloche.SetActive(true);
            }

            else if(selected == btnOptions)
            {
                AllSelectionsFalse();
                optionsCloche.SetActive(true);
            }

            else if (selected == btnCredits)
            {
                AllSelectionsFalse();
                creditsCloche.SetActive(true);
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

            if (Input.GetKeyDown(KeyCode.X))
            {
                uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);

                if (selected == btnStart)
                {
                    StartGame();
                }

                else if (selected == btnLevels)
                {
                    Levels();
                }

                else if (selected == btnTutorial)
                {
                    canvas.GetComponent<MenuManager>().ToLevel("Tutorial");
                }

                else if (selected == btnOptions)
                {
                    SceneManager.LoadScene("Options", LoadSceneMode.Additive);
                    showingOptions = true;
                    eventSystem.enabled = false;
                }

                else if (selected == btnCredits)
                {
                    Credits();
                }

                else if (selected == btnQuit)
                {
                    ConfirmQuit();
                }
            }

            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Escape))
            {
                uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);

                ConfirmQuit();
            }
        }

        // if the quit confirmation is showing
        else if (showingQuitConf && !showingOptions)
        {
            if (selected != btnYesQuit && selected != btnNoQuit)
            {
                selected = btnNoQuit;
            }

            if (selected == btnYesQuit)
            {
                yesQuitCloche.SetActive(true);
                noQuitCloche.SetActive(false);
            }

            else if (selected == btnNoQuit)
            {
                yesQuitCloche.SetActive(false);
                noQuitCloche.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);

                if (selected == btnYesQuit)
                {
                    QuitGame();
                }

                else if (selected == btnNoQuit)
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

        lastSelected = selected;
    }

    // user presses start game
    private void StartGame()
    {
        canvas.GetComponent<MenuManager>().ToVideo("intro");
    }

    // user presses levels
    private void Levels()
    {
        canvas.GetComponent<MenuManager>().ToLevels();
    }

    // user presses credits
    private void Credits()
    {
        // SceneManager.LoadSceneAsync("Credits");
        canvas.GetComponent<MenuManager>().ToCredits();
    }

    // user presses quit game, give a quit confirmation popup
    // to verify that this is what they want
    private void ConfirmQuit()
    {
        showingQuitConf = true;
        eventSystem.SetSelectedGameObject(btnNoQuit);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void Back()
    {
        showingQuitConf = false;
        eventSystem.SetSelectedGameObject(btnStart);
    }

    // helper function to turn off all cloches
    private void AllSelectionsFalse()
    {
        startCloche.SetActive(false);
        levelsCloche.SetActive(false);
        tutorialCloche.SetActive(false);
        optionsCloche.SetActive(false);
        creditsCloche.SetActive(false);
        quitCloche.SetActive(false);
    }
}
