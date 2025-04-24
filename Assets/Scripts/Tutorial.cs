using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

public class Tutorial : MonoBehaviour
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

    // for pausing when a tooltip is showing
    private bool pauseTimer;

    [SerializeField] private GameObject welcomeTooltip, welcome2Tooltip;
    [SerializeField] private GameObject moveTooltip, jumpTooltip, slideTooltip, sensorTooltip, elevatorTooltip, goTo3Tooltip, webTooltip, fallTooltip, takeOrderTooltip;
    [SerializeField] private GameObject waitOrderTooltip, watchPatienceTooltip, patiencePenaltyTooltip, foodReadyTooltip, deliverTooltip, moneyTooltip;
    [SerializeField] private GameObject timeTooltip, timeLimitTooltip, finalTooltip;

    [SerializeField] private GameObject elevator;

    private int state = 0;
    private GameObject currentTooltip;

    private float playerX, playerY;
    [SerializeField] GameObject player;

    private bool skippable = false;
    private bool tooltipComplete = false;
    private bool orderTaken = false, orderReady = false, orderPickedUp = false, orderDelivered = false;

    private bool transitioning = false;

    void Awake() 
    {
        Time.timeScale = 1f;

        soundManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundManager>();
        
        sceneName = SceneManager.GetActiveScene().name;

        tooltipComplete = false;
        orderTaken = false;
        orderReady = false;
        orderDelivered = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        goal.text = "Goal: $" + dailyGoal;
        // hour.localRotation = Quaternion.Euler(0, 0, 0);

        // StaticData.goals[sceneName] = dailyGoal;
        StaticData.goal = dailyGoal;
        StaticData.justPlayed = sceneName;

        elevator.GetComponent<UpDownChunk>().ToSpecificFloor("Floor 2");

        // TutorialController();
    }

    // Update is called once per frame
    void Update()
    {
        skippable = false;
        playerX = player.transform.localPosition.x;
        playerY = player.transform.localPosition.y;

        DisplayTime();
        ClockDisplay(timeOfDay);

        // money.text = "Money: $" + (Mathf.Round(dailyTotal * 100)) / 100.0;
        money.text = "Money: $" + GetMoneyFormat(dailyTotal);
        
        if (!pauseTimer && timeUntilLevelOver > 0)
        {
            timeUntilLevelOver -= Time.deltaTime;
        }

        if (isLevelOver && !transitioning)
        {
            transitioning = true;

            soundManager.PlaySoundEffect(soundManager.endOfDaySound);
            soundManager.PauseMusic();

            // Should freeze most things in the game
            Time.timeScale = 0f;


            // add to static data to keep track of data across scenes
            StaticData.score = dailyTotal;
            StaticData.highestSatisfaction = highestSatisfaction;
            StaticData.toPostGame = true;

            // mark tutorial as completed for this player
            StaticData.tutorialCompleted = true;

            StartCoroutine(WaitTransition());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Pause();
            }
            
            else
            {
                Unpause();
            }
        }

        // disable the old tooltip
        if (currentTooltip != null)
        {
            currentTooltip.SetActive(false);
        }

        switch (state)
        {
            case 0:
                currentTooltip = welcomeTooltip;
                skippable = true;

                if (!tooltipComplete)
                {
                    StartCoroutine(Wait5Seconds());
                }

                break;

            case 1:
                currentTooltip = welcome2Tooltip;
                skippable = true;

                if (!tooltipComplete)
                {
                    StartCoroutine(Wait5Seconds());
                }

                break;

            case 2: // move left right
                currentTooltip = moveTooltip;
                skippable = false;

                if (!tooltipComplete && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    StartCoroutine(WaitBetweenToolTips());
                }

                break;

            case 3: // jump
                currentTooltip = jumpTooltip;
                skippable = false;

                if (!tooltipComplete && Input.GetKeyDown(KeyCode.X))
                {
                    StartCoroutine(WaitBetweenToolTips());
                }

                break;

            case 4: // slide
                currentTooltip = slideTooltip;
                skippable = false;

                if (!tooltipComplete && Input.GetKeyDown(KeyCode.DownArrow))
                {
                    StartCoroutine(WaitBetweenToolTips());
                }

                break;

            case 5: // sensor
                currentTooltip = sensorTooltip;
                skippable = false;

                if (!tooltipComplete && playerX >= 6f)
                {
                    StartCoroutine(WaitBetweenToolTips());
                }

                break;

            case 6: // elevator
                currentTooltip = elevatorTooltip;
                skippable = false;

                if (!tooltipComplete && Input.GetKeyDown(KeyCode.UpArrow))
                {
                    StartCoroutine(WaitBetweenToolTips());
                }

                break;

            case 7: // go to floor 3
                currentTooltip = goTo3Tooltip;
                skippable = false;

                if (!tooltipComplete && player.GetComponent<PlayerMovement>().GetFloorNum() == 3)
                {
                    StartCoroutine(WaitBetweenToolTips());
                }

                break;

            case 8: // web
                currentTooltip = webTooltip;
                skippable = false;

                if (!tooltipComplete && playerX <= 3.5f && player.GetComponent<PlayerMovement>().GetFloorNum() == 3)
                {
                    StartCoroutine(WaitBetweenToolTips());
                }

                break;

            case 9: // hole
                currentTooltip = fallTooltip;
                skippable = false;

                if (!tooltipComplete && playerX <= -2f && player.GetComponent<PlayerMovement>().GetFloorNum() == 3)
                {
                    StartCoroutine(WaitBetweenToolTips());
                }

                break;

            case 10:
                currentTooltip = takeOrderTooltip;
                skippable = false;

                // print(orderTaken);

                if (!tooltipComplete && orderTaken)
                {
                    StartCoroutine(WaitBetweenToolTips());
                }

                break;

            case 11:
                currentTooltip = waitOrderTooltip;
                skippable = true;
                
                if (!tooltipComplete)
                {
                    StartCoroutine(Wait5Seconds());
                }

                break;

            case 12:
                currentTooltip = watchPatienceTooltip;
                skippable = true;

                if (!tooltipComplete)
                {
                    StartCoroutine(Wait5Seconds());
                }

                break;

            case 13:
                currentTooltip = patiencePenaltyTooltip;
                skippable = false;
                
                if (!tooltipComplete && orderReady)
                {
                    StartCoroutine(WaitBetweenToolTips());
                }

                break;

            case 14:
                currentTooltip = foodReadyTooltip;
                skippable = false;

                if (!tooltipComplete && orderPickedUp)
                {
                    StartCoroutine(WaitBetweenToolTips());
                }

                break;

            case 15:
                currentTooltip = deliverTooltip;
                skippable = false;

                if (!tooltipComplete && orderDelivered)
                {
                    StartCoroutine(WaitBetweenToolTips());
                }

                break;

            case 16:
                currentTooltip = moneyTooltip;
                skippable = true;

                if (!tooltipComplete)
                {
                    StartCoroutine(Wait5Seconds());
                }

                break;

            case 17:
                currentTooltip = timeTooltip;
                skippable = true;

                if (!tooltipComplete)
                {
                    StartCoroutine(Wait5Seconds());
                }

                break;

            case 18:
                currentTooltip = timeLimitTooltip;
                skippable = true;

                if (!tooltipComplete)
                {
                    StartCoroutine(Wait5Seconds());
                }

                break;

            case 19:
                currentTooltip = finalTooltip;
                skippable = true;

                if (!tooltipComplete)
                {
                    StartCoroutine(Wait5Seconds());
                }

                break;

            case 20:
                isLevelOver = true;
                break;

            default:
                currentTooltip = null;
                break;
        }

        // enable the new tooltip
        if (currentTooltip != null)
        {
            currentTooltip.SetActive(true);
        }

        // press space to skip
        if (Input.GetKeyDown(KeyCode.Space) && skippable)
        {
            state++;
        }

        // cheat for testing to skip to end of time
        if (Input.GetKeyDown(KeyCode.T))
        {
            isLevelOver = true;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            isLevelOver = true;
            dailyTotal = dailyGoal;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            state = 6;
        }
    }

    public void AddTableTip(float satisfaction)
    {
        orderDelivered = true;

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

    private IEnumerator WaitTransition()
    {
        yield return new WaitForSecondsRealtime(5);

        SceneManager.LoadSceneAsync("Menus");
    }

    private void Pause()
    {
        paused = true;
        Time.timeScale = 0f;

        // load the pause screen 
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
    }

    private void Unpause()
    {
        // unload the pause screen
        int n = SceneManager.sceneCount;

        if (n > 1)
        {
            SceneManager.UnloadSceneAsync("Options");
        }
        
        paused = false;
        Time.timeScale = 1f;
    }

    private void TooltipController()
    {
        
    }

    private IEnumerator WaitBetweenToolTips()
    {
        tooltipComplete = true;
        currentTooltip = null; // disable the tooltip

        yield return new WaitForSeconds(0.5f);

        state++; // show the next tooltip
        tooltipComplete = false;
    }

    private IEnumerator Wait5Seconds()
    {
        tooltipComplete = true;
        yield return new WaitForSeconds(5);
        state++;
        tooltipComplete = false;
    }

    public void OrderTaken()
    {
        // Debug.Log("ordertaken called!");
        orderTaken = true;
    }

    public void OrderReady()
    {
        orderReady = true;
    }

    public void OrderPickedUp()
    {
        orderPickedUp = true;
    }
}
