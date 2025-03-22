using TMPro;
using UnityEngine;

public class UpDownChunk : MonoBehaviour
{
    private int level = 0;
    private Vector3 targetPosition;
    private Vector3 target;

    // [SerializeField] private Transform touch;
    // [SerializeField] private Transform ele;
    // [SerializeField] private LayerMask eleLayer;
    // [SerializeField] private LayerMask groundLayer;
    // [SerializeField] private Rigidbody2D rb;

    private GameObject player;

    [SerializeField] private Transform groundFloor;
    [SerializeField] private Transform floor2;
    [SerializeField] private Transform floor3;


    private float moveSpeed = 15f; // Speed of smooth movement
    private float sensorSpeed = 8f;
    private bool callingElevator = false;

    private void Start()
    {
        // Set initial target position
        targetPosition = transform.position;
        player = GameObject.Find("Player");
    }

    // private bool IsTouching()
    // {
    //     return Physics2D.OverlapCircle(touch.position, 0.2f, eleLayer);
    // }

    // private bool GroundCheck()
    // {
    //     return Physics2D.OverlapCircle(touch.position, 0.2f, groundLayer);
    // }

    void Update()
    {
        // Smoothly move towards the target position
        if (callingElevator)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, sensorSpeed * Time.deltaTime);
            target = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            target = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        }
    }

    private void Up()
    {
        callingElevator = false;

        if (Mathf.Approximately(transform.position.y, targetPosition.y) && level < 2)
            {
                if (level == 0)
                {
                    targetPosition = new Vector3(transform.position.x, floor2.position.y , transform.position.z);
                }

                else if (level == 1)
                {
                    targetPosition = new Vector3(transform.position.x, floor3.position.y, transform.position.z);

                }

                player.SendMessage("FollowElevator", target);

                level += 1;
            }
    }

    private void Down()
    {
        callingElevator = false;

        if (Mathf.Approximately(transform.position.y, targetPosition.y))
        {
            if (level == 0)
            {
                targetPosition = new Vector3(transform.position.x, groundFloor.position.y, transform.position.z);
            }

            else if (level == 1)
            {
                targetPosition = new Vector3(transform.position.x, floor2.position.y, transform.position.z);
            }

            player.SendMessage("FollowElevator", target);

            level -= 1;
        }
    }

    // called when the player is using a sensor plate to call the elevator
    // to a floor that it is not on
    private void ToSpecificFloor(string name)
    {
        callingElevator = true;

        if (Mathf.Approximately(transform.position.y, targetPosition.y))
        {
            if (name.Contains("Ground"))
            {
                targetPosition = new Vector3(transform.position.x, groundFloor.position.y, transform.position.z);
                level = 0;
            }

            else if (name.Contains("2"))
            {
                targetPosition = new Vector3(transform.position.x, floor2.position.y, transform.position.z);
                level = 1;
            }

            else if (name.Contains("3"))
            {
                targetPosition = new Vector3(transform.position.x, floor3.position.y, transform.position.z);
                level = 2;
            }
        }
    }
}