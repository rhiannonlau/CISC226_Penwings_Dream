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
    private GameObject btnStart, btnLevels, btnControls, btnCredits, btnQuit;

    // references to the cloches for indicating which button is being selected
    private GameObject startCloche, levelsCloche, creditsCloche, controlsCloche, quitCloche;

    // true when the controls panel is showing
    private bool showingControls;

    // controlling the quit confirmation pop up
    private GameObject quitPopUp;
    private bool showingQuitConf;
    private GameObject yesQuit, noQuit;

    public void Start()
    {
        lastSelected = btnStart;

        // get the references to the buttons
        btnStart = transform.GetChild(1).gameObject;
        btnLevels = transform.GetChild(2).gameObject;
        btnControls = transform.GetChild(3).gameObject;
        btnCredits = transform.GetChild(4).gameObject;
        btnQuit = transform.GetChild(5).gameObject;

        // get the references to their cloches
        startCloche = btnStart.transform.GetChild(0).gameObject;
        levelsCloche = btnLevels.transform.GetChild(0).gameObject;
        controlsCloche = btnControls.transform.GetChild(0).gameObject;
        creditsCloche = btnCredits.transform.GetChild(0).gameObject;
        quitCloche = btnQuit.transform.GetChild(0).gameObject;

        // start with Start Game selected
        AllSelectionsFalse();
        startCloche.SetActive(true);
        EventSystem.current.SetSelectedGameObject(btnStart);

        showingControls = false;

        // quit pop up hidden
        quitPopUp = transform.GetChild(7).gameObject;
        showingQuitConf = false;

        // get the yes and no options for the quit confirmation
        yesQuit = quitPopUp.transform.GetChild(1).gameObject;
        noQuit = quitPopUp.transform.GetChild(2).gameObject;
    }

    public void Update()
    {
        // get the current selection
        selected = EventSystem.current.currentSelectedGameObject;

        // to catch edge cases where mouse deselects all options
        // reset the selected option to be the last known selection
        if (!selected)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }

        quitPopUp.SetActive(showingQuitConf);

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

        else if(selected == btnControls)
        {
            AllSelectionsFalse();
            controlsCloche.SetActive(true);
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

        // if the quit confirmation is showing
        if (showingQuitConf && !showingControls)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (selected == yesQuit)
                {
                    QuitGame();
                }

                else if (selected == noQuit)
                {
                    Back();
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Back();
            }
        }

        if (!showingQuitConf && !showingControls)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (selected == btnStart)
                {
                    StartGame();
                }

                else if(selected == btnLevels)
                {
                    Levels();
                }

                else if(selected == btnControls)
                {
                    ShowControls();
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

            if (Input.GetKeyDown(KeyCode.Z))
            {
                ConfirmQuit();
            }
        }

        if (showingControls && Input.GetKeyDown(KeyCode.Z))
        {
            HideControls();
        }

        lastSelected = selected;
    }

    // user presses start game
    private void StartGame()
    {
        // SceneManager.LoadSceneAsync("LoadingScreen");
        canvas.GetComponent<MenuManager>().StartGame();
    }

    // user presses levels
    private void Levels()
    {
        // SceneManager.LoadSceneAsync("Levels");
        canvas.GetComponent<MenuManager>().ToLevels();
    }

    private void ShowControls()
    {
        SceneManager.LoadScene("Controls", LoadSceneMode.Additive);
        showingControls = true;
    }

    private void HideControls()
    {
        int n = SceneManager.sceneCount;

        if (n > 1)
        {
            SceneManager.UnloadSceneAsync("Controls");
        }

        showingControls = false;
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
        EventSystem.current.SetSelectedGameObject(noQuit);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void Back()
    {
        showingQuitConf = false;
        EventSystem.current.SetSelectedGameObject(btnStart);
    }

    // helper function to turn off all cloches
    private void AllSelectionsFalse()
    {
        startCloche.SetActive(false);
        levelsCloche.SetActive(false);
        controlsCloche.SetActive(false);
        creditsCloche.SetActive(false);
        quitCloche.SetActive(false);
    }
}
