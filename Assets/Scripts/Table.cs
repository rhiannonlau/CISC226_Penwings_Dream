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

    // References the kitchen counter objects' positionality and such
    public Transform counter;

    // For currentState == 1
    public float ponderingMenuRate = 2;
    private float ponderingTimer = 0;

    public GameObject[] foodOptions;
    public GameObject[] foodOrders;
    public GameObject customerOrder;

    public Transform customerTable;

    // For currentState == 2
    private PlayerMovement playerMovement;
    private bool isPlayerNear = false;

    // For currentState == 3
    public float cookingRate = 5;
    private float cookingTimer = 0;
    public int selectedFoodItem = -1;
    
    // For currentState == 4
    // public bool isPlayerHoldingFood = false;
    // public float foodDropRadius = 5f;
    // Vector3 tableTop;
    // // public GameObject correctFoodItem;
    // // public Collider customerTableCollider;
    // public GameObject heldFoodItem;

    public GameObject heldFoodItem;
    public bool isFoodDelivered = false;
    Vector3 tableTop;
    private Renderer[] speechBubbleRender;

    // For currentState == 5
    public float eatingRate = 5;
    private float eatingTimer = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        
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
            if (ponderingTimer < ponderingMenuRate)
            {
                ponderingTimer += Time.deltaTime;
            }

            // If timer has met or exceeded the spawn rate, then spawn the order and start the time again
            else
            {
                ponderingTimer = 0;

                selectedFoodItem = Random.Range(0, foodOptions.Length);
                // Debug.Log("Random out is: " + selectedFoodItem);

                Vector3 offset = new Vector3(0, 1.26f, 0);

                // if (selectedFoodItem == 0)
                // {
                //     Instantiate(foodOrders[0], customerTable.position + offset, customerTable.rotation);
                // }
                // else if (selectedFoodItem == 1)
                // {
                //     Instantiate(foodOrders[1], customerTable.position + offset, customerTable.rotation);
                // }
                // else if (selectedFoodItem == 2)
                // {
                //     Instantiate(foodOrders[2], customerTable.position + offset, customerTable.rotation);
                // }
            
                customerOrder = Instantiate(foodOrders[selectedFoodItem], customerTable.position + offset, customerTable.rotation);

                speechBubbleRender = customerOrder.GetComponentsInChildren<Renderer>();

                foreach (Renderer r in speechBubbleRender)
                {
                    r.enabled = true;
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
            if (cookingTimer < cookingRate)
            {
                cookingTimer += Time.deltaTime;
            }

            // If timer has met or exceeded the spawn rate, then spawn the order and reset timer
            else
            {
                spawnFoodItem(foodOptions[selectedFoodItem], counter);
                cookingTimer = 0;
                currentState = 4;
            }
        }

        // 4 = deliver food to customer's table and make sure its the correct table's order
        else if (currentState == 4)
        {

            if (Input.GetKeyDown(KeyCode.W) == true && isFoodDelivered == true)
            {
                Destroy(heldFoodItem);
                // speechBubbleRender.enabled = false;
                foreach (Renderer r in speechBubbleRender)
                {
                    r.enabled = false;
                }

                isFoodDelivered = false;
                heldFoodItem = null;
                currentState = 5;
            }
        }

        // 5 = customer has received their food and is currently eating
        else if (currentState == 5)
        {
            if (eatingTimer < eatingRate)
            {
                eatingTimer += Time.deltaTime;
            }

            // If timer has met or exceeded the spawn rate, then customer is done eating so reset timer and move to the next state
            else
            {
                eatingTimer = 0;
                currentState = 6;
            }
        }

        // 6 = customers have finished their meal and have left the table
        else if (currentState == 6)
        {
            // TODO: add post-meal customer interactions (like leaving the table and resuming wandering) and money interactions

            // Set currentState back to 0 to start the cycle over again
            currentState = 0;
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

        if (collider.CompareTag("Food"))
        {
            heldFoodItem = collider.gameObject;
            isFoodDelivered = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerNear = false;
        }

        if (collider.CompareTag("Food"))
        {
            isFoodDelivered = false;
            heldFoodItem = null;
        }
    }

}
