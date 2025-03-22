using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    // Time and condition variables for "pondering" (when customer is "thinking" about what to order)
    public float ponderingMenuRate = 2;
    private float ponderingTimer = 0;
    public bool isPondering = false;

    // Time and condition variables for "pondering" (when "kitchen" is making the order)
    public float cookingRate = 5;
    private float cookingTimer = 0;
    public bool isCooking = false;
    
    // Array of speech bubble renderer components
    private Renderer[] speechBubbleRender;


    // Update is called once per frame
    void Update()
    {
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
                Vector3 offset = new Vector3(0, 1.26f, 0);

                // Spawns speech bubble of customer's order above the table
                customerOrder = Instantiate(foodOrders[selectedFoodItem], customerTable.position + offset, customerTable.rotation);

                // Gets all renderer references of game object and its children
                speechBubbleRender = customerOrder.GetComponentsInChildren<Renderer>();

                // Enable the speech bubble renderers
                foreach (Renderer r in speechBubbleRender)
                {
                    r.enabled = true;
                }
                
                // Set the table's state to show that they are ready to order
                state = 1;

                isPondering = false;

            }
        }

        if (isCooking == true)
        {
            // After customer order gets taken, the order gets prepared by kitchen for x amount of time
            if (cookingTimer < cookingRate)
            {
                cookingTimer += Time.deltaTime;
            }

            // If timer has met or exceeded the spawn rate, then spawn customer's order on the kitchen counter and reset timer
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
        GameObject newFood = Instantiate(foodItem, counter.position + Vector3.up, counter.rotation);
        
        // make the food sit on the counter and able to be passed through
        // but still able to be picked up by the player
        // newFood.transform.SetParent(counter, true); // to not fall through counter
        newFood.GetComponent<Collider2D>().isTrigger = true; // to let player pass through
        Rigidbody2D newFoodRb = newFood.GetComponent<Rigidbody2D>();
        newFoodRb.bodyType = RigidbodyType2D.Static; // to sit on counter

        // make it so the food doesn't slide infinitely on the floor
        newFoodRb.drag = 2.5f;
    }

    // When player meets conditions to take customer order, the customer's selected menu item reference is sent back to player
    void TakeOrder()
    {
        customerFoodSelection = foodOptions[selectedFoodItem];
        isCooking = true;

        // Set the table's state to show that they are waiting for their order
        state = 2;
    }

    // If order is being delivered to the correct table and other player-based conditions are met, the speech bubble disappears 
    void DeliverOrder() // Renderer[] customerSpeechBubble
    {
        // Disable the speech bubble renderers
        foreach (Renderer r in speechBubbleRender)
        {
            r.enabled = false;
        }

        // the order is correct
        if (CheckOrder())
        {
            state = 0; // neutral state
            
            // disable the food's collider so it can no longer be picked up
            Collider2D foodColl = transform.GetChild(0).GetComponent<Collider2D>();
            foodColl.enabled = false;

            Rigidbody2D foodRb = transform.GetChild(0).GetComponent<Rigidbody2D>();
            foodRb.simulated = false;
        }
        // otherwise, it stays in the waiting for order state
        // so that the user can pick up the order again

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
}
