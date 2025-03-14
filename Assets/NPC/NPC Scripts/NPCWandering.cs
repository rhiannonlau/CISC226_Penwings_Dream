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

    // Start is called before the first frame update
    void Start()
    {
        
        randomtime = Random.Range(minwalk, maxwalk); // randomized walking time
        ani.SetBool("iswalk", iswalk ? true : false); // animation swithcing
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= randomtime) // idle
        {
            change();
        }
        // boundary constraints
        if (!isflip && (transform.position.x > rightboundX || transform.position.x < leftboundX))
            StartCoroutine(Flip());
        
        if(iswalk)
            rb.velocity = Vector2.right * facingDirection * speed;

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
}
