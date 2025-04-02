using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    void Awake() 
    {
        soundManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        goal.text = "Goal: $" + dailyGoal;

        // add to the main manager to keep track of across scenes
        if (MainManager.Instance)
        {
            MainManager.Instance.Goal = dailyGoal;
        }
    }

    // Update is called once per frame
    void Update()
    {
        DisplayTime();

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

            // Should freeze most things in the game
            Time.timeScale = 0f;

            // add to the main manager to keep track of across scenes
            MainManager.Instance.Score = dailyTotal;
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

    void DisplayTime()
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
}
