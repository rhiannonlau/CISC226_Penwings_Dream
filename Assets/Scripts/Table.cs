using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO

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

        // 1 = customers sit at the table but they have not yet put in their order
        else if (currentState == 1)
        {
            
        }

        // 2 = customers are ready to place their order
        else if (currentState == 2)
        {

        }

        // 3 = order has been placed by the player
        else if (currentState == 3)
        {

        }

        // 4 = kitchen has finished cooking the food and food is ready to be picked up by player for delivery
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
}
