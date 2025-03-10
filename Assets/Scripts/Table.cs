using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Table : MonoBehaviour
{

    public bool isCustomerPresent = false;

    // Current state for each table (where are they in the order sequence)
    // // 0 = there are no customers at the table
    // // 1 = customers sit at the table but they have not yet put in their order
    // // 2 = customers are ready to place their order
    // // 3 = order has been placed by the player
    // // 4 = kitchen has finished cooking the food and food is ready to be picked up by player for delivery
    // // 5 = customer has received their food and is currently eating
    // // 6 = customers have finished their meal and have left the table
    public int currentState = 0;

    // public GameObject fishPlatter;
    // public GameObject fishSoup;
    // public GameObject veggieBurger;

    public GameObject food;

    // References the kitchen counter objects' positionality and such
    public Transform counter;

    // For currentState == 1
    public float ponderingMenuSpawnRate = 2;
    private float ponderingTimer = 0;

    // For currentState == 3
    public float cookTimeSpawnRate = 5;
    private float cookingTimer = 0;

    // For currentState == 2
    public GameObject[] foodOptions;
    public GameObject[] foodOrders;

    public Transform tableFurniture;

    private PlayerMovement playerMovement;
    private bool isPlayerNear = false;

    // Start is called before the first frame update
    void Start()
    {
        // spawnFoodItem(food, counter);
    }

    // Update is called once per frame
    void Update()
    {

        // 0 = there are no customers at the table (ie. the customers are wandering and not sitting down)
        if (currentState == 0)
        {

            // TODO: DELETE LATER
            currentState = 1;

            if (this.isCustomerPresent == true)
            {
                currentState = 1;
            }

        }

        // 1 = customers sit at the table and place their order
        else if (currentState == 1)
        {
            // If the timer is less than the spawn rate, then we want to make the timer count up by one
            if (ponderingTimer < ponderingMenuSpawnRate)
            {
                ponderingTimer += Time.deltaTime;
            }

            // If timer has met or exceeded the spawn rate, then spawn the order and start the time again
            else
            {
                ponderingTimer = 0;

                int randomFoodIndex = Random.Range(0, foodOptions.Length);
                // Debug.Log("Random out is: " + randomFoodIndex);

                if (randomFoodIndex == 0)
                {
                    Instantiate(foodOrders[0], tableFurniture.position + Vector3.up, tableFurniture.rotation);
                }
                else if (randomFoodIndex == 1)
                {
                    Instantiate(foodOrders[1], tableFurniture.position + Vector3.up, tableFurniture.rotation);
                }
                else if (randomFoodIndex == 2)
                {
                    Instantiate(foodOrders[2], tableFurniture.position + Vector3.up, tableFurniture.rotation);
                }

                currentState = 2;
            }
        }

        // 2 = customer order gets taken by player if player is near or at the table and if they press z
        else if (currentState == 2)
        {
            if (Input.GetKeyDown(KeyCode.Z) == true && isPlayerNear == true)
            {
                currentState = 3;
            }
        }

        // 3 = order gets sent to kitchen to cook
        else if (currentState == 3)
        {
            // If the timer is less than the spawn rate, then we want to make the timer count up by one
            if (cookingTimer < cookTimeSpawnRate)
            {
                cookingTimer += Time.deltaTime;
            }

            // If timer has met or exceeded the spawn rate, then spawn the order and start the time again
            else
            {
                spawnFoodItem(food, counter);
                cookingTimer = 0;
                currentState = 4;
            }
        }

        // 4 = food is ready to be picked up by player for delivery
        else if (currentState == 4)
        {

        }

        // 5 = customer has received their food and is currently eating
        else if (currentState == 5)
        {

        }

        // 6 = customers have finished their meal and have left the table
        else if (currentState == 6)
        {

        }

    }

    // Call this function to set whether a customer is at a table or not.
    void setCustomerAtTable(bool isPresent)
    {
        this.isCustomerPresent = isPresent;
    }

    void spawnFoodItem(GameObject foodItem, Transform counter)
    {
        Instantiate(foodItem, counter.position + Vector3.up, counter.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Checks if its the player trying to interact with the speech bubble and not a different GameObject
        if (collider.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }


}
