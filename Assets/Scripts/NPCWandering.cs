using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Tilemaps;
using UnityEngine;

public class NPCWandering : MonoBehaviour
{
    // general movement
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float leftboundX, rightboundX;
    [SerializeField] private Animator ani;

    // random intervals of movement
    [SerializeField] private float minpause, maxpause;
    [SerializeField] private float minwalk, maxwalk;

    [SerializeField] private int facingDirection = 1;

    private float randomtime, timer;
    private bool iswalk = true;
    private bool isflip;
    private bool wander = true;

    [SerializeField] private bool isHungry = false;
    [SerializeField] private bool isSitting = false;
    [SerializeField] private bool isEating = false;
    [SerializeField] private float timeUntilHungry;
    [SerializeField] private float hungerMin = 5;
    [SerializeField] private float hungerMax = 20;
    [SerializeField] private float timeUntilDoneEating;
    public Table tableInteractions;
    [SerializeField] private Transform chair;

    // Start is called before the first frame update
    void Start()
    {
        randomtime = Random.Range(minwalk, maxwalk); // randomized walking time
        ani.SetBool("iswalk", iswalk ? true : false); // animation switching
        SetHungerTime();

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (wander && isSitting == false)
        {
            if (timer >= randomtime) // idle
            {
                change();
            }
            // boundary constraints
            if (!isflip && (transform.position.x > rightboundX || transform.position.x < leftboundX))
                StartCoroutine(Flip());

            if (iswalk)
                rb.velocity = Vector2.right * facingDirection * speed;
        }

        if (timeUntilHungry > 0)
        {
            timeUntilHungry -= Time.deltaTime;
        }

        else if (isHungry == false)
        {
            isHungry = true;
        }

        if (timeUntilDoneEating > 0)
        {
            timeUntilDoneEating -= Time.deltaTime;
        }
        
        else if (isEating == true)
        {
            isEating = false;
            isSitting = false;
            ani.SetBool("iswalk", true);
            ani.SetBool("isSitting", false);
            ani.SetBool("isEating", false);
            SetHungerTime();
            tableInteractions.isPondering = false;
            // call destroy plate function from Table.cs after I make that
            Vector3 sitPosition = transform.position;
            sitPosition.y -= 0.4f;
            transform.position = sitPosition;

            GetComponent<Collider2D>().enabled = true;

        }

    }

    public void StopWandering()
    {
        wander = false;
        // Stop any movement logic
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void StartWandering()
    {
        wander = true;
        // Resume wandering logic
    }

    // moves in opposite direction
    IEnumerator Flip()
    {
        isflip = true;
        transform.Rotate(0, 180, 0);
        facingDirection *= -1;
        yield return new WaitForSeconds(0.5f);
        isflip = false;
    }
    // switches between walking and idle
    void change()
    {
        iswalk = !iswalk;
        ani.SetBool("iswalk", iswalk ? true : false);
        randomtime = iswalk ? Random.Range(minwalk, maxwalk) : Random.Range(minpause, maxpause);
        timer = 0;
    }

    void SetHungerTime()
    {
        timeUntilHungry = Random.Range(hungerMin, hungerMax);
        isHungry = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Chair"))
        {
            // Debug.Log("has collided with chair");

            if (isHungry == true)
            {
                if (isSitting == false)
                {
                    isSitting = true;
                    ani.SetBool("isSitting", true);

                    Vector3 sitPosition = transform.position;
                    sitPosition.x = chair.position.x;
                    sitPosition.y += 0.4f;
                    transform.position = sitPosition;
                    rb.velocity = new Vector3(0, 0, 0);
                    
                    if (facingDirection < 0)
                    {
                        StartCoroutine(Flip());
                    }

                    tableInteractions.isPondering = true;

                    GetComponent<Collider2D>().enabled = false;
                }
            }
        }
    }

    public void FoodDelivered()
    {
        isEating = true;
        ani.SetBool("isEating", true);
        timeUntilDoneEating = 5f;
    }

}