using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    [SerializeField] private bool isLevelOver = false;
    [SerializeField] private float timeUntilLevelOver = 120f;
    [SerializeField] private float dailyTotal;
    [SerializeField] private const float BASE_COST = 10f;
    [SerializeField] private int numTablesServed = 0;
    [SerializeField] private float highestSatisfaction = 0f;
    [SerializeField] private int timeOfDay = 0;
    // [SerializeField] private bool isAM = true;
    [SerializeField] private float dailyGoal = 70f;

    public SoundManager soundManager;

    public TMP_Text time;
    public TMP_Text goal;
    public TMP_Text money;

    public Transform hour;
    public Transform minute;

    private string sceneName;

    // for pausing the game
    private bool paused;

    private GameObject hotelSign;

    void Awake() 
    {
        Time.timeScale = 1f;

        soundManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundManager>();
        
        sceneName = SceneManager.GetActiveScene().name;

        hotelSign = GameObject.Find("Sign");
    }

    // Start is called before the first frame update
    void Start()
    {
        goal.text = "Goal: $" + dailyGoal;

        // StaticData.goals[sceneName] = dailyGoal;
        StaticData.goal = dailyGoal;
        StaticData.justPlayed = sceneName;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayTime();
        ClockDisplay(timeOfDay);

        // money.text = "Money: $" + (Mathf.Round(dailyTotal * 100)) / 100.0;
        money.text = "Money: $" + GetMoneyFormat(dailyTotal);
        
        if (timeUntilLevelOver > 0)
        {
            timeUntilLevelOver -= Time.deltaTime;
        }

        else if (isLevelOver == false)
        {
            isLevelOver = true;

            soundManager.PlaySoundEffect(soundManager.endOfDaySound);

            soundManager.StopMusic();

            hotelSign.GetComponent<HotelSignController>().TurnOff();

            // Should freeze most things in the game
            Time.timeScale = 0f;


            // add to static data to keep track of data across scenes
            StaticData.score = dailyTotal;
            StaticData.highestSatisfaction = highestSatisfaction;

            // keep track of the high scores for each level (if we have time)
            // if (StaticData.highscores[sceneName] < dailyTotal)
            // {
            //     try
            //     {
            //         StaticData.highscores[sceneName] = dailyTotal;
            //     }

            //     catch (ArgumentException)
            //     {
            //         StaticData.highscores.Add("sceneName", dailyTotal);
            //     }
                
            // }
            
            // if (StaticData.highestSatisfactions[sceneName] < highestSatisfaction)
            // {
            //     StaticData.highestSatisfactions[sceneName] = highestSatisfaction;
            // }
            
            StaticData.toPostGame = true;

            
            StartCoroutine(WaitExitGame());
        }

        // pause the game when the user presses the escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                // pause game bgm
                soundManager.StopMusic();

                paused = true;
                Time.timeScale = 0f;

                // load the pause screen 
                SceneManager.LoadScene("PauseScreen", LoadSceneMode.Additive);

                // get a reference to the pause screen's PauseGame script
                PauseGame p = GameObject.Find("PauseScreen").GetComponent<PauseGame>();
                p.GetBaseInfo(this, sceneName); // pass a reference to this gm script and the name of this scene
            }
            
            else
            {
                Unpause();
            }
        }

        // cheat for testing to skip to end of time
        if (Input.GetKeyDown(KeyCode.T))
        {
            timeUntilLevelOver = 1;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            timeUntilLevelOver = 1;
            dailyTotal = dailyGoal;
        }
    }

    public void AddTableTip(float satisfaction)
    {
        float tableTip = 0;

        if (satisfaction >= 0.75f)
        {
            tableTip = satisfaction * 5f;
        }

        else if (satisfaction >= 0.5f)
        {
            tableTip = satisfaction * 8f;
        }

        else if (satisfaction >= 0.25f)
        {
            tableTip = satisfaction * 10f;
        }

        dailyTotal += BASE_COST + tableTip;

        numTablesServed += 1;

        if (satisfaction > highestSatisfaction)
        {
            highestSatisfaction = satisfaction;
        }
    }

    public string GetMoneyFormat(float moneyAmount)
    {
        return moneyAmount.ToString("F2");
    }

    public void DisplayTime()
    {
        if (timeUntilLevelOver >= 105f)
        {
            timeOfDay = 9;
            time.text = "Time: 9AM";
        }

        else if (timeUntilLevelOver >= 90f)
        {
            timeOfDay = 10;
            time.text = "Time: 10AM";
        }

        else if (timeUntilLevelOver >= 75f)
        {
            timeOfDay = 11;
            time.text = "Time: 11AM";
        }

        else if (timeUntilLevelOver >= 60f)
        {
            timeOfDay = 12;
            // isAM = false;
            time.text = "Time: 12PM";
        }

        else if (timeUntilLevelOver >= 45f)
        {
            timeOfDay = 1;
            time.text = "Time: 1PM";
        }

        else if (timeUntilLevelOver >= 30f)
        {
            timeOfDay = 2;
            time.text = "Time: 2PM";
        }

        else if (timeUntilLevelOver >= 15f)
        {
            timeOfDay = 3;
            time.text = "Time: 3PM";
        }

        else if (timeUntilLevelOver >= 0f)
        {
            timeOfDay = 4;
            time.text = "Time: 4PM";
        }
        
        else
        {
            timeOfDay = 5;
            time.text = "Time: 5PM";
        }

    }

    public void ClockDisplay(int dayTime)
    {
        float hourPosition = dayTime * 30f;

        hour.localRotation = Quaternion.Euler(0, 0, -hourPosition);
        minute.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private IEnumerator WaitExitGame()
    {
        yield return new WaitForSecondsRealtime(4);

        SceneManager.LoadSceneAsync("Menus");
    }

    public void Unpause()
    {
        // unload the pause screen
        int n = SceneManager.sceneCount;

        if (n > 1)
        {
            SceneManager.UnloadSceneAsync("PauseScreen");
        }

        paused = false;
        Time.timeScale = 1f;

        // play game bgm
        soundManager.PlayMusic();
    }
}
