using UnityEngine;
using TMPro; // For TextMeshPro

public class GameManager : MonoBehaviour
{
    public TMP_Text moneyText;       // UI Text for money
    public TMP_Text timerText;       // UI Text for timer
    public GameObject resultsPanel;  // Results panel (set inactive by default)
    public TMP_Text resultsText;     // Results text inside the panel

    private int money = 0;           // Player's money
    private float timer = 60f;       // Countdown timer (60 seconds)
    private bool gameRunning = true;

    void Start()
{
    // Manually find TimerText if it's missing
    if (timerText == null)
    {
        timerText = GameObject.Find("timerText").GetComponent<TMP_Text>();
    }

    // Manually find MoneyText
    if (moneyText == null)
    {
        moneyText = GameObject.Find("moneyText").GetComponent<TMP_Text>();
    }

    // Manually find ResultsPanel
    if (resultsPanel == null)
    {
        resultsPanel = GameObject.Find("resultsPanel");
    }

    // Manually find ResultsText
    if (resultsText == null)
    {
        resultsText = GameObject.Find("resultsText").GetComponent<TMP_Text>();
    }

    resultsPanel.SetActive(false); // Hide results panel at the start
    UpdateMoneyText();
    UpdateTimerText();
    StartGame();
}


void Update()
{
    Debug.Log("🔥 Update is running! gameRunning = " + gameRunning); // ✅ Debug line

    if (gameRunning)  
    {
        timer -= Time.deltaTime; 
        Debug.Log("⏳ Timer is now: " + timer); // ✅ Debugging the timer
        UpdateTimerText();

        if (timer <= 0) 
        {
            EndGame();
        }
    }
}

    // Subtract money from total 
    public void SubtractMoney(int amount)
    {
        money -= amount;
        UpdateMoneyText();
    }

    // Adds money to the total
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
    }

    // Updates the money display
    void UpdateMoneyText()
    {
        moneyText.text = "Money: $" + money;
    }

    // Updates the timer display
    void UpdateTimerText()
    {
        timerText.text = $"Time: {Mathf.Ceil(timer)}s";
    }

    // Ends the game and shows results
    void EndGame()
    {
        gameRunning = false; // Stop the timer
        resultsPanel.SetActive(true); // Show the results
        resultsText.text = "You earned: $" + money + "\n" + 
                           (money >= 50 ? "You progress to the next day!" : "Try again!");
    }

    // Starts the game
public void StartGame()
{
    Debug.Log("🚀 Game Started! Timer should run.");
    gameRunning = true;  // ✅ Ensure this is TRUE
    timer = 60f;         
    money = 0;
    resultsPanel.SetActive(false);
    UpdateMoneyText();
    UpdateTimerText();
}
}