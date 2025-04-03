using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBounds : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -6f)
        {
            RespawnFoodObj();
        }
    }

    void RespawnFoodObj()
    {
        Vector3 respawnPos = new Vector3(0, -4, 0);
        transform.position = respawnPos;
    }
}
