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
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        target = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
    }

    private void Up()
    {
        if (Mathf.Approximately(transform.position.y, targetPosition.y) && level < 2)
            {
                if(level == 0)
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
}