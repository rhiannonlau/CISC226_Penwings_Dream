using System.ComponentModel;
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

        lastSelected = btnLvl1;
        // Debug.Log(lastSelected);
    }

    // reset everything when enabled again
    void OnEnable()
    {
        AllSelectionsFalse();

        // start with the last option that was selected
        EventSystem.current.SetSelectedGameObject(lastSelected);

        print(lastSelected);
        print(selected);
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

        // if selected is not = to a button in this panel and lastSelected is null, reset to btnLvl1
        else if (!(selected == btnLvl1 || selected == btnLvl2 || selected == btnLvl3 || selected == btnLvl4 || selected == btnLvl5 || selected == btnMainMenu) && !lastSelected)
        {
            EventSystem.current.SetSelectedGameObject(btnLvl1);
        }

        // depending on which option is selected, show the cloche
        else if (selected == btnLvl1)
        {
            AllSelectionsFalse();
            lvl1Cloche.SetActive(true);
        }

        else if(selected == btnLvl2)
        {
            AllSelectionsFalse();
            lvl2Cloche.SetActive(true);
        }

        else if(selected == btnLvl3)
        {
            AllSelectionsFalse();
            lvl3Cloche.SetActive(true);
        }

        else if (selected == btnLvl4)
        {
            AllSelectionsFalse();
            lvl4Cloche.SetActive(true);
        }

        else if (selected == btnLvl5)
        {
            AllSelectionsFalse();
            lvl5Cloche.SetActive(true);
        }

        else if (selected == btnMainMenu)
        {
            AllSelectionsFalse();
            mainMenuCloche.SetActive(true);
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
                canvas.GetComponent<MenuManager>().ToLevel("Level 1");
            }

            else if(selected == btnLvl2)
            {
                canvas.GetComponent<MenuManager>().ToLevel("Level 2");
            }

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

    private void MainMenu()
    {
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
