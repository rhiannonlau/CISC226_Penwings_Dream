using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Table : MonoBehaviour
{

    public Transform counter;
    public Transform customerTable;

    public GameObject[] foodOptions;
    public GameObject[] foodOrders;
    public int selectedFoodItem = -1;
    public GameObject customerFoodSelection;
    public GameObject customerOrder;

    public float ponderingMenuRate = 2;
    private float ponderingTimer = 0;
    public bool isPondering = false;
    public float cookingRate = 5;
    private float cookingTimer = 0;
    public bool isCooking = false;
    
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

    void SpawnFoodItem(GameObject foodItem, Transform counter)
    {
        Instantiate(foodItem, counter.position + Vector3.up, counter.rotation);
    }

    void TakeOrder(GameObject player)
    {
        customerFoodSelection = foodOptions[selectedFoodItem];
        player.SendMessage("", customerFoodSelection);
        isCooking = true;
    }

    void DeliverOrder(Renderer[] customerSpeechBubble)
    {
        foreach (Renderer r in customerSpeechBubble)
        {
            r.enabled = false;
        }
    }

}
