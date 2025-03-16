using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Table : MonoBehaviour
{

    // Transform for kitchen counters and customer tables
    public Transform counter;
    public Transform customerTable;

    // Arrays of game objects and their corresponding speech bubbles
    public GameObject[] foodOptions;
    public GameObject[] foodOrders;

    // Variables for ordering
    public int selectedFoodItem = -1;
    public GameObject customerFoodSelection;
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
        Instantiate(foodItem, counter.position + Vector3.up, counter.rotation);
    }

    // When player meets conditions to take customer order, the customer's selected menu item reference is sent back to player
    void TakeOrder(GameObject player)
    {
        customerFoodSelection = foodOptions[selectedFoodItem];
        player.SendMessage("", customerFoodSelection);
        isCooking = true;
    }

    // If order is being delivered to the correct table and other player-based conditions are met, the speech bubble disappears 
    void DeliverOrder(Renderer[] customerSpeechBubble)
    {
        // Disable the speech bubble renderers
        foreach (Renderer r in customerSpeechBubble)
        {
            r.enabled = false;
        }
    }

}
