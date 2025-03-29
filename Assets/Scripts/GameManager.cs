using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private bool isLevelOver = false;
    [SerializeField] private float timeUntilLevelOver = 120f;
    [SerializeField] private float dailyTotal;
    [SerializeField] private const float BASE_COST = 10f;
    [SerializeField] private int numTablesServed = 0;
    [SerializeField] private float highestSatisfaction = 0f;
    [SerializeField] private int timeOfDay = 0;
    [SerializeField] private bool isAM = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        displayTime();
        
        if (timeUntilLevelOver > 0)
        {
            timeUntilLevelOver -= Time.deltaTime;
        }

        else if (isLevelOver == false)
        {
            isLevelOver = true;

            // Should freeze most things in the game
            Time.timeScale = 0f;
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

    void displayTime()
    {
        if (timeUntilLevelOver >= 105f)
        {
            timeOfDay = 9;
        }

        else if (timeUntilLevelOver >= 90f)
        {
            timeOfDay = 10;
        }

        else if (timeUntilLevelOver >= 75f)
        {
            timeOfDay = 11;
        }

        else if (timeUntilLevelOver >= 60f)
        {
            timeOfDay = 12;
            isAM = false;
        }

        else if (timeUntilLevelOver >= 45f)
        {
            timeOfDay = 1;
        }

        else if (timeUntilLevelOver >= 30f)
        {
            timeOfDay = 2;
        }

        else if (timeUntilLevelOver >= 15f)
        {
            timeOfDay = 3;
        }

        else if (timeUntilLevelOver >= 0f)
        {
            timeOfDay = 4;
        }
        
        else
        {
            timeOfDay = 5;
        }


    }
}
