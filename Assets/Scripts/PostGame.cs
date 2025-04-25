using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostGame : MonoBehaviour
{
    // reference canvas to access MenuManager.cs to change panels
    [SerializeField] private Canvas canvas;

    // reference to the transform
    private RectTransform tr;

    // reference to the text
    private TMP_Text txtStats;
    private float goal, score, highestSatisfaction;
    private string result;

    // the name of the level that was just played
    private string justPlayed;

    // references to the selected button
    private GameObject selected, lastSelected;

    // references to the buttons
    private GameObject btnNextLevel, btnNext, btnRetry, btnMainMenu;
    private Button mainMenu, active;
    private Navigation nav;
    
    private GameObject btnActive;

    // references to the cloches for indicating which button is being selected
    private GameObject nextLevelCloche, nextCloche, retryCloche, mainMenuCloche;
    // this serves as a reference to the cloche of whichever button out of
    // btnNextLevel and btnRetry is active
    private GameObject activeCloche;

    // store whether the player completed the level or not
    private bool completedLevel;

    void Awake()
    {
        tr = GetComponent<RectTransform>();
        txtStats = tr.GetChild(1).GetComponent<TMP_Text>();

        // get the references to the buttons
        btnNextLevel = transform.GetChild(2).gameObject;
        btnNext = transform.GetChild(3).gameObject;
        btnRetry = transform.GetChild(4).gameObject;
        btnMainMenu = transform.GetChild(5).gameObject;
        
        // get the references to their cloches
        nextLevelCloche = btnNextLevel.transform.GetChild(1).gameObject;
        nextCloche = btnNext.transform.GetChild(1).gameObject;
        retryCloche = btnRetry.transform.GetChild(1).gameObject;
        mainMenuCloche = btnMainMenu.transform.GetChild(1).gameObject;

        lastSelected = btnMainMenu;
    }

    // reset everything when enabled again
    void OnEnable()
    {
        AllSelectionsFalse();
        btnNextLevel.SetActive(false);
        btnNext.SetActive(false);
        btnRetry.SetActive(false);

        completedLevel = CompletedLevel();

        if (completedLevel && justPlayed != "Level 5")
        {
            btnActive = btnNextLevel;
            activeCloche = nextLevelCloche;
        }

        else if (completedLevel && justPlayed == "Level 5")
        {
            btnActive = btnNext;
            activeCloche = nextCloche;
        }

        else // they failed the level
        {
            btnActive = btnRetry;
            activeCloche = retryCloche;
        }

        // setting the mode of the main menu button explicitly
        // so that it can properly navigate depending on which button is active
        mainMenu = btnMainMenu.GetComponent<Button>();
        active = btnActive.GetComponent<Button>();
        nav.mode = Navigation.Mode.Explicit;
        nav.selectOnUp = active;
        nav.selectOnLeft = active;
        nav.selectOnDown = active;
        nav.selectOnRight = active;
        mainMenu.navigation = nav;

        EventSystem.current.SetSelectedGameObject(btnActive);
        btnActive.SetActive(true);

        if (completedLevel)
        {
            result = justPlayed + " Completed!";
        }

        else
        {
            result = justPlayed + " Failed";
        }

        txtStats.text = "Today's Goal: $" + goal.ToString() + "\nYour Score: $" + score.ToString("F2") + "\nHighest Satisfaction: " + (highestSatisfaction * 100).ToString("F2")+ "%\n\nResult: " + result;
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
        else if (!(selected == btnNextLevel || selected == btnRetry || selected == btnNext || selected == btnMainMenu) && !lastSelected)
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
                if (completedLevel && justPlayed != "Level 5")
                {
                    if (justPlayed != "Tutorial")
                    {
                        // get the level's number as a char
                        char justPlayedNum = justPlayed[6];

                        // convert to int and increment by one
                        int nextLevelNum = justPlayedNum - '0';
                        nextLevelNum++;

                        canvas.GetComponent<MenuManager>().ToLevel("Level " + nextLevelNum.ToString());
                    }
                    
                    else
                    {
                        canvas.GetComponent<MenuManager>().ToLevel("Level 1");
                    }
                }

                else if (completedLevel && justPlayed == "Level 5")
                {
                    canvas.GetComponent<MenuManager>().ToVideo("outro");
                }

                else
                {
                    canvas.GetComponent<MenuManager>().ToLevel("Level " + justPlayed[6].ToString());
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

    void MainMenu()
    {
        canvas.GetComponent<MenuManager>().ToMainMenu();
    }

    public bool CompletedLevel()
    {
        goal = StaticData.goal;
        score = StaticData.score;
        highestSatisfaction = StaticData.highestSatisfaction;
        justPlayed = StaticData.justPlayed;

        return score >= goal;
    }

    // helper function to turn off all cloches
    private void AllSelectionsFalse()
    {
        nextCloche.SetActive(false);
        retryCloche.SetActive(false);
        mainMenuCloche.SetActive(false);
        nextLevelCloche.SetActive(false);
    }
}
