using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;

    [SerializeField] private float speed; // seralizeField makes it editable from unity ui
    private float originalSpeed;
    [SerializeField] private float jumpPower;
    private float originalJumpPower;

    private float horizontalInput;

    // how long the penalty lasts
    [SerializeField] private float penaltyLength;
    float startTime;

    // to track if a penalty is active
    private bool frozen = false; // version 1
    private bool sluggish = false; // version 2
    private bool holdingFood = false; // version 3

    // cheat codes to speed up prototype testing
    private bool version1 = false;
    private bool version2 = false;
    private bool version3 = false;


    // Variables for grabbing
    [SerializeField] private Transform grabPoint;

    [SerializeField] private Transform rayPoint;
    [SerializeField] private float rayDistance;

    private GameObject grabbedObject;
    private int layerIndex;

    private void Start()
    {
        layerIndex = LayerMask.NameToLayer("Food");
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

        // to be able to return to original speed and jumpPower after penalty effects
        originalSpeed = speed;
        originalJumpPower = jumpPower;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // for moving
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance); // for grabbing

        // for testing: allow the player to choose which prototype version in game
        if (Input.GetKey(KeyCode.Alpha1))
        {
            NoVersion();
            version1 = true;
            Debug.Log("Version 1");
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            NoVersion();
            version2 = true;
            Debug.Log("Version 2");
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            NoVersion();
            version3 = true;
            Debug.Log("Version 3");
        }

        // game logic
        // if the player is frozen, they cannot make any actions
        if (!frozen)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            // space bar to jump
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }

            // Grabbing
            if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)
            {
                // w to grab object
                if (Keyboard.current.wKey.wasPressedThisFrame && grabbedObject == null)
                {
                    grabbedObject = hitInfo.collider.gameObject;
                    grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    grabbedObject.transform.position = grabPoint.position;
                    grabbedObject.transform.SetParent(transform);
                    holdingFood = true;
                }

                // w to release object
                else if (Keyboard.current.wKey.wasPressedThisFrame)
                {
                    DropFood();
                }

                // Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
            }
        }

        else
        {
            // make the sprite blink by turning the renderer on and off after a certain delay using invokerepeating
            // don't use frames bc frame rate differs by device
        }

        // for testing: reset the player's position and all settings
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
            Debug.Log("OnCollisionEnter2D");

            if (version1)
            {
                Freeze();
            }

            else if (version2)
            {
                Slow();
            }

            else if (version3)
            {
                DropFood();
            }
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
        NoVersion();
    }

    // reset version
    private void NoVersion()
    {
        version1 = false;
        version2 = false;
        version3 = false;
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

            // speed = 2f;
            // jumpPower = 2f;
            body.drag = 50f;
        }
    }

    // version 3: player drops the food if they were holding any
    private void DropFood()
    {
        if (holdingFood)
        {
            holdingFood = false;
            grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
        }
    }

    // remove any debuffs
    private void EndPenalty()
    {
        if (frozen) { frozen = false; }

        if (sluggish)
        {
            sluggish = false;
            // speed = originalSpeed;
            // jumpPower = originalJumpPower;
            body.drag = 0f;
        }
    }
}
