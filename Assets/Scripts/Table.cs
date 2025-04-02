using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Table : MonoBehaviour
{
    public int state;
    public int State
    {
        get { return state; }
        set { state = value; }
    }

    // Transform for kitchen counters and customer tables
    public Transform counter;
    public Transform customerTable;

    // Arrays of game objects and their corresponding speech bubbles
    public GameObject[] foodOptions;
    public GameObject[] foodOrders;

    // Variables for ordering
    public int selectedFoodItem = 0;
    public GameObject customerFoodSelection {get; private set;}
    public GameObject customerOrder;
    public GameObject ticket;
    public GameObject newTicket;
    public Transform stovetop;
    public float ticketSpeed = 5;

    // Time and condition variables for "pondering" (when customer is "thinking" about what to order)
    public float ponderingMenuRate = 2;
    private float ponderingTimer = 0;
    public bool isPondering = false;

    public bool isOrderSending = false;

    // Time and condition variables for "cooking" (when "kitchen" is making the order)
    public float cookingRate = 5;
    private float cookingTimer = 0;
    public bool isCooking = false;
    
    // Array of speech bubble renderer components
    private SpriteRenderer[] speechBubbleRender;

    public NPCWandering npc;

    [SerializeField] private const float PATIENCE_TIME = 50;
    [SerializeField] private float patienceCountdown = 0;
    [SerializeField] private GameObject uiBarContainer;
    [SerializeField] private SpriteRenderer uiBarFill;

    public GameManager gameManager;

    // Run at startup
    void Start() {
        // Ensure patience bar is hidden
        uiBarContainer.SetActive(false);
    }

    float getPatience() {
        float patience = patienceCountdown/(float)PATIENCE_TIME;
        if (patience < 0) {
            patience = 0;
        }
        return patience;
    }

    // Update is called once per frame
    void Update()
    {

        // update patience counter + bar
        if (state == 1 || state == 2) {
            patienceCountdown -= Time.deltaTime;
            
            float patience = getPatience();
            // TODO: Fix scaling
            uiBarFill.size = new Vector2(patience * 1.8f, uiBarFill.size.y);
        }

        if (isPondering == true)
        {
            // Customers sit at the table, pondering the menu for set amount of time before ordering
            if (ponderingTimer < ponderingMenuRate)
            {
                ponderingTimer += Time.deltaTime;
            }

            // If timer has met or exceeded the spawn rate, then spawn the speech bubble and restart timer
            else
            {
                ponderingTimer = 0;

                // // Customers' orders are randomly selected from the menu
                selectedFoodItem = Random.Range(0, foodOptions.Length);

                // Position offset from table for spawning of speech bubble
                Vector3 offset = new Vector3(0, 0.9f, 0);

                // Spawns speech bubble of customer's order above the table
                customerOrder = Instantiate(foodOrders[selectedFoodItem], customerTable.position + offset, customerTable.rotation);

                // Gets all renderer references of game object and its children
                speechBubbleRender = customerOrder.GetComponentsInChildren<SpriteRenderer>();

                // Enable the speech bubble renderers
                foreach (SpriteRenderer r in speechBubbleRender)
                {
                    if (r == speechBubbleRender[speechBubbleRender.Length - 1])
                    {
                        r.enabled = false;
                    }
                    
                    else
                    {
                        r.enabled = true;
                    }

                }
                
                // Set the table's state to show that they are ready to order
                state = 1;

                isPondering = false;

                // Make patience bar visible
                uiBarContainer.SetActive(true);
                patienceCountdown = PATIENCE_TIME;

            }
        }

        if (isOrderSending == true)
        {
            ticketSpeed = 7;
            newTicket.transform.position = Vector3.MoveTowards(newTicket.transform.position, stovetop.transform.position, ticketSpeed * Time.deltaTime);
            if (newTicket.transform.position == stovetop.transform.position)
            {
                Debug.Log("ticket has been moved");
                Destroy(newTicket);
                isOrderSending = false;
            }
            
        }
          
        if (isCooking == true)
        {
            // After customer order gets taken, the order gets prepared by kitchen for x amount of time
            if (cookingTimer < cookingRate)
            {
                cookingTimer += Time.deltaTime;
            }

            // If timer has met or exceeded the cooking rate, then spawn customer's order on the kitchen counter and reset timer
            else
            {
                SpawnFoodItem(foodOptions[selectedFoodItem], counter);
                cookingTimer = 0;
                isCooking = false;
            }
        }

    }

    // Spawns the correct order for a table on top of its corresponding kitchen counter when it's done "cooking"
    void SpawnFoodItem(GameObject foodItem, Transform counter)
    {
        Vector3 offset = new Vector3(0, 0.9f, 0);

        GameObject newFood = Instantiate(foodItem, counter.position + offset, counter.rotation);
        
        // make the food sit on the counter and able to be passed through
        // but still able to be picked up by the player
        // newFood.transform.SetParent(counter, true); // to not fall through counter
        newFood.GetComponent<Collider2D>().isTrigger = true; // to let player pass through
        Rigidbody2D newFoodRb = newFood.GetComponent<Rigidbody2D>();
        newFoodRb.bodyType = RigidbodyType2D.Static; // to sit on counter

        // make it so the food doesn't slide infinitely on the floor
        newFoodRb.drag = 2.5f;
    }

    void SpawnTicket()
    {
        newTicket = Instantiate(ticket, customerTable.position + Vector3.up, customerTable.rotation);
        isOrderSending = true;
    }

    // When player meets conditions to take customer order, the customer's selected menu item reference is sent back to player
    void TakeOrder()
    {
        customerFoodSelection = foodOptions[selectedFoodItem];
        SpawnTicket();
        isCooking = true;

        // When player takes the customer's order, the opacity of the speech bubble changes (50% transparent) to indicate that order has been taken
        foreach (SpriteRenderer r in speechBubbleRender)
        {
            r.color = new Color(1f, 1f, 1f, 0.5f);
        }

        // Set the table's state to show that they are waiting for their order
        state = 2;
    }

    // Checks and acts in accordance to whether correct or incorrect order was delivered to the table
    // If correct order was delivered, speech bubble disappears and the food is not able to be picked back up
    // If incorrect order was delivered, a question mark appears over the speech bubble and the food is still able to be picked up
    void DeliverOrder() // Renderer[] customerSpeechBubble
    {
        // If the order is correct
        if (CheckOrder())
        {
            state = 0; // neutral state

            // Disable the speech bubble renderers
            foreach (SpriteRenderer r in speechBubbleRender)
            {
                r.enabled = false;
            }
            
            // disable the food's collider so it can no longer be picked up
            Collider2D foodColl = transform.GetChild(0).GetComponent<Collider2D>();
            foodColl.enabled = false;

            Rigidbody2D foodRb = transform.GetChild(0).GetComponent<Rigidbody2D>();
            foodRb.simulated = false;

            npc.FoodDelivered();

            // hide patience bar
            uiBarContainer.SetActive(false);

            Debug.Log($"NPC Patience at delivery: {getPatience()*100:F1}%");

            gameManager.AddTableTip(getPatience());
        }

        // If the order is wrong
        else
        {
            // Table state indicates that customers are still waiting for their CORRECT order
            state = 2;

            // Enable the speech bubble renderers
            foreach (SpriteRenderer r in speechBubbleRender)
            {
                if (r == speechBubbleRender[speechBubbleRender.Length - 1])
                {
                    r.color = new Color(1f, 0f, 0f, 1f);
                    r.enabled = true;
                }
            }

            // enable the food's collider so it can still be picked up since the order is wrong
            Collider2D foodColl = transform.GetChild(0).GetComponent<Collider2D>();
            foodColl.enabled = true;

            Rigidbody2D foodRb = transform.GetChild(0).GetComponent<Rigidbody2D>();
            foodRb.simulated = true;
        }

        Debug.Log(state);
    }

    // code for checking if the order is the right one
    private bool CheckOrder()
    {
        // table should only have one child: the food object
        // so we get the reference to that object by its index in the child hierarchy
        Transform foodObject = transform.GetChild(0);

        // get the names of the objects to check equivalence
        // (the food the player is holding will have the same name,
        // but have "(clone)" at the end)
        string wantedType = customerFoodSelection.name;
        string holdingType = foodObject.name;

        // check if this table's food selection is same as the one the player is holding
        // return true if it is, false otherwise
        return holdingType.Contains(wantedType);
    }

    public void FinishedEating()
    {
        Destroy(transform.GetChild(0).gameObject);
    }
}
