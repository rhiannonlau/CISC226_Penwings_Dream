using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PostGame : MonoBehaviour
{
    // reference to the transform
    private RectTransform tr;

    // reference to the text
    private TMP_Text txtStats;
    private float goal, score, highestSatisfaction;
    private string result;

    // references to the selected button
    private GameObject selected, lastSelected;

    // references to the buttons
    private GameObject btnNext, btnRetry, btnMainMenu;
    
    private GameObject btnActive;

    // references to the cloches for indicating which button is being selected
    private GameObject nextCloche, retryCloche, mainMenuCloche;
    // this serves as a reference to the cloche of whichever button out of
    // btnNext and btnRetry is active
    private GameObject activeCloche;

    // store whether the player completed the level or not
    private bool completedLevel;

    void Awake()
    {
        tr = GetComponent<RectTransform>();
        txtStats = tr.GetChild(1).GetComponent<TMP_Text>();

        // get the references to the buttons
        btnNext = transform.GetChild(2).gameObject;
        btnRetry = transform.GetChild(3).gameObject;
        btnMainMenu = transform.GetChild(4).gameObject;
        
        // get the references to their cloches
        nextCloche = btnNext.transform.GetChild(0).gameObject;
        retryCloche = btnRetry.transform.GetChild(0).gameObject;
        mainMenuCloche = btnMainMenu.transform.GetChild(0).gameObject;

        lastSelected = btnMainMenu;
    }

    // reset everything when enabled again
    void OnEnable()
    {
        AllSelectionsFalse();
        btnNext.SetActive(false);
        btnRetry.SetActive(false);

        completedLevel = CompletedLevel();

        if (completedLevel)
        {
            btnActive = btnNext;
            activeCloche = nextCloche;
        }

        else // they failed the level
        {
            btnActive = btnRetry;
            activeCloche = retryCloche;
        }

        EventSystem.current.SetSelectedGameObject(btnActive);

        // setting the text
        // if (GetStats())
        // {
        //     if (completedLevel)
        //     {
        //         result = "Level Completed!";
        //     }

        //     else
        //     {
        //         result = "Level Failed";
        //     }

        //     txtStats.text = "Today's Goal: " + goal.ToString() + "/nYour Score: " + score.ToString() + "/nResult: " + result;
        // }

        // else
        // {
        //     txtStats.text = "Error";
        // }

        if (completedLevel)
        {
            result = "Level Completed!";
        }

        else
        {
            result = "Level Failed";
        }

        txtStats.text = "Today's Goal: " + goal.ToString() + "/nYour Score: " + score.ToString() + "/nResult: " + result;

        // else
        // {
        //     txtStats.text = "Error";
        // }
    }

    void Update()
    {
        // get the current selection
        selected = EventSystem.current.currentSelectedGameObject;

        // to catch edge cases where mouse deselects all options
        // reset the selected option to be the last known selection
        if (!selected)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }

        // if selected is not = to a button in this panel and lastSelected is null, reset to btnStart
        else if (!(selected == btnNext || selected == btnRetry || selected == btnMainMenu) && !lastSelected)
        {
            EventSystem.current.SetSelectedGameObject(btnActive);
        }

        // depending on which option is currently being hovered, show the cloche
        if (selected == btnActive)
        {
            AllSelectionsFalse();
            activeCloche.SetActive(true);
        }

        else if(selected == btnMainMenu)
        {
            AllSelectionsFalse();
            mainMenuCloche.SetActive(true);
        }

        else
        {
            AllSelectionsFalse();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (selected == btnActive)
            {
                if (completedLevel)
                {
                    NextLevel();
                }

                else
                {
                    RetryLevel();
                }
            }

            else if(selected == btnMainMenu)
            {
                MainMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            MainMenu();
        }
        

        lastSelected = selected;
    }

    void NextLevel()
    {

    }

    void RetryLevel()
    {

    }

    void MainMenu()
    {
        // SceneManager.LoadSceneAsync("Menus");
    }

    public bool GetStats()
    {
        // if (MainManager.Instance)
        // {
        //     goal = MainManager.Instance.Goal;
        //     score = MainManager.Instance.Score;

        //     completedLevel = score >= goal;

        //     return true;
        // }

        return false;
        
    }

    public bool CompletedLevel()
    {
        goal = StaticData.goal;
        score = StaticData.score;
        highestSatisfaction = StaticData.highestSatisfaction;

        return score >= goal;
    }

    // helper function to turn off all cloches
    private void AllSelectionsFalse()
    {
        retryCloche.SetActive(false);
        mainMenuCloche.SetActive(false);
        nextCloche.SetActive(false);
    }
}
