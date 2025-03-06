using UnityEngine;

public class numbers : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int level = 0;
    private Vector3 targetPosition;
    [SerializeField] private Transform touch;
    [SerializeField] private Transform ele;
    [SerializeField] private LayerMask eleLayer;

    private float moveSpeed = 16f; // Speed of smooth movement
    void Start()
    {
        targetPosition = ele.position;
    }

    private bool IsTouching()
    {
        return Physics2D.OverlapCircle(touch.position, 0.2f, eleLayer);
    }


    // Update is called once per frame
    void Update()
    {
        if (IsTouching() && Mathf.Approximately(ele.position.y, targetPosition.y))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                level = 0;
                targetPosition = new Vector3(ele.position.x, level * 3, ele.position.z);
            }
            else if ((Input.GetKeyDown(KeyCode.Alpha2)))
            {
                level = 1;
                targetPosition = new Vector3(ele.position.x, level * 3, ele.position.z);
            }
            else if ((Input.GetKeyDown(KeyCode.Alpha3)))
            {
                level = 2;
                targetPosition = new Vector3(ele.position.x, level * 3, ele.position.z);
            }
        
        }

        ele.position = Vector3.MoveTowards(ele.position, targetPosition, moveSpeed * Time.deltaTime);
        
    }
}
