using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;

    [SerializeField] private float speed; // seralizeField makes it editable from unity
    private float originalSpeed;
    [SerializeField] private float jumpPower;
    private float originalJumpPower;

    private float horizontalInput;

    // how long the penalty lasts
    [SerializeField] private float penaltyLength;
    float startTime;

    [SerializeField] private bool frozen = false; // version 1
    private bool sluggish = false; // version 2
    private bool holdingFood = false; // version 3

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

        // to be able to return to original speed and jumpPower after penalty effects
        originalSpeed = speed;
        originalJumpPower = jumpPower;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (!frozen)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }

        else
        {
            // make the sprite blink by turning the renderer on and off after a certain delay using invokerepeating
            // don't use frames bc frame rate differs by device
        }

        if (Input.GetKey(KeyCode.R))
        {
            Reset();
        }

        // check if the penalty time has passed
        if ((frozen || sluggish) && Time.time - startTime >= penaltyLength)
        {
            EndPenalty();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground Floor")
        {
            Freeze();
            Debug.Log("OnCollisionEnter2D");
        }
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
    }

    // reset to original settings for testing
    private void Reset()
    {
        transform.position = new Vector2(-5, 1);
        speed = originalSpeed;
        jumpPower = originalJumpPower;
        sluggish = false;
        frozen = false;
    }

    // version 1: player freezes for x seconds
    private void Freeze()
    {
        if (!frozen)
        {
            startTime = Time.time;
            frozen = true;
        }
    }

    // version 2: player is sluggish for x seconds
    private void Slow()
    {
        // if the player is not already sluggish
        if (!sluggish)
        {
            startTime = Time.time;
            sluggish = true;

            speed = 0.5f;
            jumpPower = 0.5f;
        }
    }

    // version 3: player drops the food if they were holding any
    private void DropFood()
    {
        if (holdingFood)
        {
            holdingFood = false;
        }
    }

    private void EndPenalty()
    {
        if (frozen) { frozen = false; }

        if (sluggish)
        {
            sluggish = false;
            speed = originalSpeed;
            jumpPower = originalJumpPower;
        }
    }
}
