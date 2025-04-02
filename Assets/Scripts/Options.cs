using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Options : MonoBehaviour
{
    // private int selected;
    private GameObject selected;
    private GameObject lastSelected;

    private GameObject btnResume;
    private GameObject btnControls;
    private GameObject btnQuit;

    private GameObject resumeCloche;
    private GameObject controlsCloche;
    private GameObject quitCloche;

    private bool showingControls;

    private GameObject quitPopUp;
    private bool showingQuitConf;

    private GameObject yesQuit;
    private GameObject noQuit;

    public void Start()
    {
        lastSelected = btnResume;

        // get the references to the buttons
        btnResume = transform.GetChild(2).gameObject;
        btnControls = transform.GetChild(3).gameObject;
        btnQuit = transform.GetChild(4).gameObject;

        // get the references to their cloches
        resumeCloche = btnResume.transform.GetChild(0).gameObject;
        controlsCloche = btnControls.transform.GetChild(0).gameObject;
        quitCloche = btnQuit.transform.GetChild(0).gameObject;

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

        quitPopUp.SetActive(showingQuitConf);

        // depending on which option is selected, show the cloche
        if (selected == btnResume)
        {
            AllSelectionsFalse();
            resumeCloche.SetActive(true);
        }

        else if(selected == btnControls)
        {
            AllSelectionsFalse();
            controlsCloche.SetActive(true);
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
                    MainMenu();
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
                if (selected == btnResume)
                {
                    ResumeGame();
                }

                else if(selected == btnControls)
                {
                    ShowControls();
                }

                else if (selected == btnQuit)
                {
                    ConfirmQuit();
                }
            }
        }

        if (showingControls && Input.GetKeyDown(KeyCode.Z))
        {
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
        controlsCloche.SetActive(false);
        quitCloche.SetActive(false);
    }
}
