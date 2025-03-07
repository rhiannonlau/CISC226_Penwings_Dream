using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created based off assumption that there is one menu item which will display itself in a thought bubble randomly over a table to convey that the customer would like to place an order
public class OrderSequence : MonoBehaviour
{
    // Used to assign number to each table depending on what floor they're on (ground floor = 1, 2nd floor = 2, etc...)
    public int tableNumber;

    // References prefab of thought bubble conveying customer order 
    public GameObject speechBubbleWithOrder;
    
    // References to customer tables
    public Transform table1;
    public Transform table2;
    public Transform table3;

    // Sets parameters for the range from which the time between orders will be determined
    public float minSpawnRate = 3;
    public float maxSpawnRate = 15;

    // Time for next order to spawn for specific table
    public float table1NextSpawn;
    public float table2NextSpawn;
    public float table3NextSpawn;

    // Timer for each table
    private float table1Timer;
    private float table2Timer;
    private float table3Timer;

    // Start is called before the first frame update
    void Start()
    {
        // Spawns random time for next order for each table
        table1NextSpawn = Random.Range(minSpawnRate, maxSpawnRate);
        table2NextSpawn = Random.Range(minSpawnRate, maxSpawnRate);
        table3NextSpawn = Random.Range(minSpawnRate, maxSpawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        // If the timer is less than the spawn rate, then we want to make the timer count up by one 
        if (table1Timer < table1NextSpawn)
        {
            table1Timer += Time.deltaTime;
        }

        // If timer has met or exceeded the spawn rate, then spawn a new order/speech bubble above that table and start the time again
        else
        {
            spawnSpeechBubble(table1);
            table1Timer = 0;

            // Determine the next random spawn time
            table1NextSpawn = Random.Range(minSpawnRate, maxSpawnRate);
        }

        if (table2Timer < table2NextSpawn)
        {
            table2Timer += Time.deltaTime;
        }
        else
        {
            spawnSpeechBubble(table2);
            table2Timer = 0;
            table2NextSpawn = Random.Range(minSpawnRate, maxSpawnRate);
        }

        if (table3Timer < table3NextSpawn)
        {
            table3Timer += Time.deltaTime;
        }
        else
        {
            spawnSpeechBubble(table3);
            table3Timer = 0;
            table3NextSpawn = Random.Range(minSpawnRate, maxSpawnRate);
        }
    }

    // Spawn speech bubble over given table
    void spawnSpeechBubble(Transform customerTable)
    {
        Instantiate(speechBubbleWithOrder, customerTable.position + Vector3.up, customerTable.rotation);
    }
}
