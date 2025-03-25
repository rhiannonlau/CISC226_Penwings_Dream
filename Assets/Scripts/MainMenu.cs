using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
    // private int selected;
    private GameObject selected;
    private GameObject lastSelected;

    private GameObject btnStart;
    private GameObject btnLevels;
    private GameObject btnCredits;
    private GameObject btnQuit;

    private GameObject startCloche;
    private GameObject levelsCloche;
    private GameObject creditsCloche;
    private GameObject quitCloche;

    private GameObject quitPopUp;
    private bool showPopUp;

    private GameObject yesQuit;
    private GameObject noQuit;

    public void Start()
    {
        lastSelected = btnStart;

        // get the references to the buttons
        btnStart = transform.GetChild(3).gameObject;
        btnLevels = transform.GetChild(4).gameObject;
        btnCredits = transform.GetChild(5).gameObject;
        btnQuit = transform.GetChild(6).gameObject;

        // get the references to their cloches
        startCloche = btnStart.transform.GetChild(0).gameObject;
        levelsCloche = btnLevels.transform.GetChild(0).gameObject;
        creditsCloche = btnCredits.transform.GetChild(0).gameObject;
        quitCloche = btnQuit.transform.GetChild(0).gameObject;

        // start with Start Game selected
        startCloche.SetActive(true);
        levelsCloche.SetActive(false);
        creditsCloche.SetActive(false);
        quitCloche.SetActive(false);

        // quit pop up hidden
        quitPopUp = transform.GetChild(8).gameObject;
        showPopUp = false;

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

        quitPopUp.SetActive(showPopUp);

        // depending on which option is selected, show the cloche
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
        if (showPopUp)
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

        else
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

                else if (selected == btnCredits)
                {
                    Credits();
                }

                else if (selected == btnQuit)
                {
                    ConfirmQuit();
                }
            }
        }

        lastSelected = selected;
    }

    // user presses start game
    public void StartGame()
    {
        Debug.Log("start game");
        SceneManager.LoadSceneAsync("LoadingScreen");
    }

    // user presses levels
    public void Levels()
    {
        
    }

    // user presses credits
    public void Credits()
    {
        SceneManager.LoadSceneAsync("Credits");
    }

    // user presses quit game, give a quit confirmation popup
    // to verify that this is what they want
    public void ConfirmQuit()
    {
        showPopUp = true;
        EventSystem.current.SetSelectedGameObject(noQuit);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Back()
    {
        showPopUp = false;
        EventSystem.current.SetSelectedGameObject(btnStart);
    }

    // helper function to turn off all cloches
    public void AllSelectionsFalse()
    {
        startCloche.SetActive(false);
        levelsCloche.SetActive(false);
        creditsCloche.SetActive(false);
        quitCloche.SetActive(false);
    }
}
