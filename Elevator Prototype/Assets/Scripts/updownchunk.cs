using UnityEngine;

public class UpDownChunk : MonoBehaviour
{
    private int level = 0;
    private Vector3 targetPosition;
    
    [SerializeField] private Transform touch;
    [SerializeField] private Transform ele;
    [SerializeField] private LayerMask eleLayer;
    [SerializeField] private LayerMask groundLayer;
    private float moveSpeed = 15f; // Speed of smooth movement

    private void Start()
    {
        // Set initial target position
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
                level += 1;
                targetPosition = new Vector3(ele.position.x, level * 3, ele.position.z);
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !GroundCheck())
            {
                level -= 1;
                targetPosition = new Vector3(ele.position.x, level * 3, ele.position.z);
            }
        }

        // Smoothly move towards the target position
        ele.position = Vector3.MoveTowards(ele.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
