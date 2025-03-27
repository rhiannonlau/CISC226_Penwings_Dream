using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour
{
    private GameObject selected;
    private GameObject lastSelected;

    private GameObject btnLvl1, btnLvl2, btnLvl3, btnLvl4, btnLvl5;
    private GameObject btnMainMenu;

    private GameObject lvl1Cloche, lvl2Cloche, lvl3Cloche, lvl4Cloche, lvl5Cloche;
    private GameObject mainMenuCloche;

    void Start()
    {
        lastSelected = btnLvl1;

        btnLvl1 = transform.GetChild(2).gameObject;
        btnLvl2 = transform.GetChild(3).gameObject;
        btnLvl3 = transform.GetChild(4).gameObject;
        btnLvl4 = transform.GetChild(5).gameObject;
        btnLvl5 = transform.GetChild(6).gameObject;
        btnMainMenu = transform.GetChild(7).gameObject;

        lvl1Cloche = btnLvl1.transform.GetChild(0).gameObject;
        lvl2Cloche = btnLvl2.transform.GetChild(0).gameObject;
        lvl3Cloche = btnLvl3.transform.GetChild(0).gameObject;
        lvl4Cloche = btnLvl4.transform.GetChild(0).gameObject;
        lvl5Cloche = btnLvl5.transform.GetChild(0).gameObject;
        mainMenuCloche = btnMainMenu.transform.GetChild(0).gameObject;

        AllSelectionsFalse();
        lvl1Cloche.SetActive(true);
    }

    void Update()
    {
        selected = EventSystem.current.currentSelectedGameObject;

        // to catch edge cases where mouse deselects all options
        // reset the selected option to be the last known selection
        if (selected == null)
        {
            Debug.Log("passed if");
            Debug.Log(lastSelected);
            EventSystem.current.SetSelectedGameObject(lastSelected);
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
            MainMenu();
        }
    }

    private void Level1()
    {
        SceneManager.LoadSceneAsync("Scene");
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
        SceneManager.LoadSceneAsync("StartMenu");
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
