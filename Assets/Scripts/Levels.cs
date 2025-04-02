using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour
{
    // reference canvas to access MenuManager.cs to change panels
    [SerializeField] private Canvas canvas;

    // references to the selected button
    private GameObject selected, lastSelected;

    // references to the buttons
    private GameObject btnLvl1, btnLvl2, btnLvl3, btnLvl4, btnLvl5, btnMainMenu;

    // references to the cloches for indicating which button is being selected
    private GameObject lvl1Cloche, lvl2Cloche, lvl3Cloche, lvl4Cloche, lvl5Cloche, mainMenuCloche;

    public UISoundManager uiSoundManager;

    void Awake() 
    {
        uiSoundManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<UISoundManager>();
    }

    void Start()
    {
        lastSelected = btnLvl1;

        // get the references to the buttons
        btnLvl1 = transform.GetChild(1).gameObject;
        btnLvl2 = transform.GetChild(2).gameObject;
        btnLvl3 = transform.GetChild(3).gameObject;
        btnLvl4 = transform.GetChild(4).gameObject;
        btnLvl5 = transform.GetChild(5).gameObject;
        btnMainMenu = transform.GetChild(6).gameObject;

        // get the references to their cloches
        lvl1Cloche = btnLvl1.transform.GetChild(0).gameObject;
        lvl2Cloche = btnLvl2.transform.GetChild(0).gameObject;
        lvl3Cloche = btnLvl3.transform.GetChild(0).gameObject;
        lvl4Cloche = btnLvl4.transform.GetChild(0).gameObject;
        lvl5Cloche = btnLvl5.transform.GetChild(0).gameObject;
        mainMenuCloche = btnMainMenu.transform.GetChild(0).gameObject;

        // start with level 1 selected
        AllSelectionsFalse();
        lvl1Cloche.SetActive(true);
        EventSystem.current.SetSelectedGameObject(btnLvl1);
    }

    void Update()
    {
        // get the current selection
        selected = EventSystem.current.currentSelectedGameObject;

        // to catch edge cases where mouse deselects all options
        // reset the selected option to be the last known selection

        if (!lastSelected)
        {
            lastSelected = btnLvl1;
        }

        if (!selected)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }

        // depending on which option is selected, show the cloche
        else if (selected == btnLvl1)
        {
            AllSelectionsFalse();
            lvl1Cloche.SetActive(true);
            Debug.Log("Lvl1");
        }

        else if(selected == btnLvl2)
        {
            AllSelectionsFalse();
            lvl2Cloche.SetActive(true);
            Debug.Log("Lvl2");
        }

        else if(selected == btnLvl3)
        {
            AllSelectionsFalse();
            lvl3Cloche.SetActive(true);
            Debug.Log("Lvl3");
        }

        else if (selected == btnLvl4)
        {
            AllSelectionsFalse();
            lvl4Cloche.SetActive(true);
            Debug.Log("Lvl4");
        }

        else if (selected == btnLvl5)
        {
            AllSelectionsFalse();
            lvl5Cloche.SetActive(true);
            Debug.Log("Lvl5");
        }

        else if (selected == btnMainMenu)
        {
            AllSelectionsFalse();
            mainMenuCloche.SetActive(true);
            Debug.Log("MM");
        }

        else
        {
            AllSelectionsFalse();
        }

        // user presses select
        if (Input.GetKeyDown(KeyCode.X))
        {
            uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);

            if (selected == btnLvl1)
            {
                Level1();
            }

            // else if(selected == btnLvl2)
            // {
            //     Level2();
            // }

            // else if(selected == btnLvl3)
            // {
            //     Level3();
            // }

            // else if (selected == btnLvl4)
            // {
            //     Level4();
            // }

            // else if (selected == btnLvl5)
            // {
            //     Level5();
            // }

            else if (selected == btnMainMenu)
            {
                MainMenu();
            }
        }

        // user presses back
        if (Input.GetKeyDown(KeyCode.Z))
        {
            uiSoundManager.PlaySoundEffect(uiSoundManager.menuSelectSound);
            
            MainMenu();
        }
    }

    private void Level1()
    {
        // SceneManager.LoadSceneAsync("Level 1");
        canvas.GetComponent<MenuManager>().ToLevel("Level 1");
    }

    // private void Level2()
    // {
    //     SceneManager.LoadSceneAsync("Level2");
    // }

    // private void Level3()
    // {
    //     SceneManager.LoadSceneAsync("Level3");
    // }

    // private void Level4()
    // {
    //     SceneManager.LoadSceneAsync("Level4");
    // }

    // private void Level5()
    // {
    //     SceneManager.LoadSceneAsync("Level5");
    // }

    private void MainMenu()
    {
        // SceneManager.LoadSceneAsync("MainMenu");
        canvas.GetComponent<MenuManager>().ToMainMenu();
    }

    private void AllSelectionsFalse()
    {
        lvl1Cloche.SetActive(false);
        lvl2Cloche.SetActive(false);
        lvl3Cloche.SetActive(false);
        lvl4Cloche.SetActive(false);
        lvl5Cloche.SetActive(false);
        mainMenuCloche.SetActive(false);
    }
}
