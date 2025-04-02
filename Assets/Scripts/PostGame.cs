using UnityEngine;
using UnityEngine.EventSystems;

public class PostGame : MonoBehaviour
{
    // references to the selected button
    private GameObject selected, lastSelected;

    // references to the buttons
    private GameObject btnNext, btnRetry, btnMainMenu;

    // references to the cloches for indicating which button is being selected
    private GameObject nextCloche, retryCloche, mainMenuCloche;

    // store whether the player completed the level or not
    private bool completedLevel;

    void Start()
    {
        lastSelected = btnMainMenu;

        // get the references to the buttons
        btnNext = transform.GetChild(1).gameObject;
        btnRetry = transform.GetChild(2).gameObject;
        btnMainMenu = transform.GetChild(3).gameObject;
        
        // get the references to their cloches
        nextCloche = btnNext.transform.GetChild(0).gameObject;
        retryCloche = btnRetry.transform.GetChild(0).gameObject;
        mainMenuCloche = btnMainMenu.transform.GetChild(0).gameObject;

        // start with Start Game selected
        AllSelectionsFalse();

        if (completedLevel)
        {
            btnRetry.SetActive(false);

            nextCloche.SetActive(true);
            EventSystem.current.SetSelectedGameObject(btnNext);
        }

        else
        {
            btnNext.SetActive(false);

            retryCloche.SetActive(true);
            EventSystem.current.SetSelectedGameObject(btnRetry);
        }
    }

    void Update()
    {
        // get the current selection
        selected = EventSystem.current.currentSelectedGameObject;

        // to catch edge cases where mouse deselects all options
        // reset the selected option to be the last known selection
        if (!lastSelected)
        {
            lastSelected = btnMainMenu;
        }

        if (!selected)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }


    }

    void NextLevel()
    {

    }

    void RetryLevel()
    {

    }

    void MainMenu()
    {

    }

    // helper function to turn off all cloches
    private void AllSelectionsFalse()
    {
        retryCloche.SetActive(false);
        mainMenuCloche.SetActive(false);
        nextCloche.SetActive(false);
    }
}
