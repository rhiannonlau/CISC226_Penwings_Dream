using UnityEngine;

public class elecheck : MonoBehaviour
{

    [SerializeField] private Transform touch;
    [SerializeField] private Transform ele;
    [SerializeField] private LayerMask eleLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private bool istouch(){
        return Physics2D.OverlapCircle(touch.position, 0.2f, eleLayer);
    }

    private bool groundCheck(){
        return Physics2D.OverlapCircle(touch.position, 0.2f, groundLayer);
    }
    // Update is called once per frame
    void Update()
    {   
        if (istouch()){
            if (Input.GetButton("Jump"))
            {
                ele.position += Vector3.up * speed * Time.deltaTime;
            }
            else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) && !groundCheck()){
                ele.position += Vector3.down * speed * Time.deltaTime;
            }

        }

    }
}
