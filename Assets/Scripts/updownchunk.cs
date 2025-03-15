using TMPro;
using UnityEngine;

public class UpDownChunk : MonoBehaviour
{
    private int level = 0;
    private Vector3 targetPosition;
    private Vector3 target;

    [SerializeField] private Transform touch;
    [SerializeField] private Transform ele;
    [SerializeField] private LayerMask eleLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Transform floor1;
    [SerializeField] private Transform floor2;
    [SerializeField] private Transform floor3;


    private float moveSpeed = 15f; // Speed of smooth movement

    private void Start()
    {
        // Set initial target position
        targetPosition = ele.position;
        targetPosition = ele.position;
    }

    private bool IsTouching()
    {
        return Physics2D.OverlapCircle(touch.position, 0.2f, eleLayer);
    }

    private bool GroundCheck()
    {
        return Physics2D.OverlapCircle(touch.position, 0.2f, groundLayer);
    }

    void Update()
    {
        if (IsTouching() && Mathf.Approximately(ele.position.y, targetPosition.y))
        {
            if (Input.GetButtonDown("Jump") && level < 2)
            {
               

                if(level == 0)
                {
                    targetPosition = new Vector3(ele.position.x, floor2.position.y , ele.position.z);
                    
                }
                else if (level == 1)
                {
                    targetPosition = new Vector3(ele.position.x, floor3.position.y, ele.position.z);
                }

                level += 1;

            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !GroundCheck())
            {
                
                if (level == 0)
                {
                    targetPosition = new Vector3(ele.position.x, floor1.position.y, ele.position.z);
                }
                else if (level == 1)
                {
                    targetPosition = new Vector3(ele.position.x, floor2.position.y, ele.position.z);
                }
                level -= 1;
            }
        }

        // Smoothly move towards the target position
        ele.position = Vector3.MoveTowards(ele.position, targetPosition, moveSpeed * Time.deltaTime);
        target = new Vector3(ele.position.x, ele.position.y + 10, ele.position.z);
        rb.position = Vector2.MoveTowards(rb.position, target, Time.deltaTime * 2);
    }
}
