using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

using UnityEngine.InputSystem;

using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    
    // variables for animation
    private Animator anim;
    private bool sliding;
    [SerializeField] private LayerMask FloorLayer;


    // movement variables
    private float horizontalInput;
    [SerializeField] private float speed; // seralizeField makes it editable from unity ui
    private float originalSpeed;
    [SerializeField] private float jumpPower;
    private float originalJumpPower;


    // sliding variables
    private BoxCollider2D coll;

    // swinging variables
    private bool swinging;
    [SerializeField] private Transform upperRayPoint;
    [SerializeField] private float upperRayDistance;
    private int chandelierLayer;
    private GameObject chandelierObject;
    private HingeJoint2D hinge;


    // penalty time length variables
    [SerializeField] private float penaltyLength;
    private float penaltyStartTime;


    // to track if a penalty is active
    private bool frozen = false; // version 1
    private bool slowed = false; // version 2
    private bool holdingFood = false; // version 3

    // private bool currentFloor; // tracks what floor the player is currently on for the falling penalty
    // i.e. on collision, if the new floor != currentfloor, issue penalty
    // how to tell if they used elevator to switch floors? 
        // elevatorswitch bool that is true when they use the elevator, which means that we would check if currentFloor != new Floor AND if elevatorSwitch?


    // ice variables
    private SpriteRenderer iceRenderer;
    [SerializeField] private GameObject ice;


    // snail variables
    private SpriteRenderer snailRenderer;
    [SerializeField] private GameObject snail;


    // Variables for grabbing
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform lowerRayPoint;
    [SerializeField] private float lowerRayDistance;
    private GameObject foodObject;
    private int foodLayer;


    private void Start()
    {
        foodLayer = LayerMask.NameToLayer("Food");
        chandelierLayer = LayerMask.NameToLayer("Chandelier");
    }

    private void Awake()
    {
        // get references from Player object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        hinge = GetComponent<HingeJoint2D>();

        // get the renderers for the special effects
        iceRenderer = ice.GetComponent<SpriteRenderer>();
        snailRenderer = snail.GetComponent<SpriteRenderer>();

        iceRenderer.enabled = false; // ice animation off
        snailRenderer.enabled = false; // snail animation off

        // to be able to return to original speed and jumpPower after penalty effects
        originalSpeed = speed;
        originalJumpPower = jumpPower;
    }

    private void Update()
    {
        bool grounded = isGrounded();

        // for swinging
        RaycastHit2D upperHitInfo = Physics2D.Raycast(upperRayPoint.position, transform.right, upperRayDistance);

        // for grabbing
        RaycastHit2D lowerHitInfo = Physics2D.Raycast(lowerRayPoint.position, transform.right, lowerRayDistance);

        // for moving
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // game logic
        AnimateMove();

        // x to jump
        if (Input.GetKeyDown(KeyCode.X) && grounded) // key down prevents them from holding down the button and floating
        {
            // if (swinging)
            // {
            //     StopSwinging(upperHitInfo);
            // }

            Jump();
        }

        // sliding
        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded())
        {
            Slide();
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) || !isGrounded())
        {
            EndSlide();
        }

        // Debug.Log(hitObject());

        // check if trying to grab while swinging
        if (upperHitInfo.collider != null && upperHitInfo.collider.gameObject.layer == chandelierLayer)
        {
            if (Input.GetKeyDown(KeyCode.Z) && chandelierObject == null && !isGrounded())
            {
                Debug.Log("swinging!");
                Swing(upperHitInfo.collider.gameObject);
            }

            
            // else if (Input.GetKeyUp(KeyCode.Z) || isGrounded())
            // {
            //     Debug.Log("key up");
            //     StopSwinging(upperHitInfo.collider.gameObject);
            // }
        }

        if (swinging)
        {
            // release z to unswing
            if (Input.GetKeyUp(KeyCode.Z) || isGrounded())
            {
                Debug.Log("key up");
                Debug.Log(isGrounded());
                StopSwinging(chandelierObject);
            }
        }


        // Picking up and putting down
        if (lowerHitInfo.collider != null && lowerHitInfo.collider.gameObject.layer == foodLayer)
        {
            // w to grab object
            if (Keyboard.current.zKey.wasPressedThisFrame && foodObject == null && isGrounded())
            {
                PickUpFood(lowerHitInfo);
            }

            // w to release object
            else if (Keyboard.current.zKey.wasPressedThisFrame && isGrounded())
            {
                DropFood(lowerHitInfo);
            }
        }

        // for testing: reset the player's position and all settings
        if (Input.GetKey(KeyCode.R))
        {
            Reset();
        }

        // check if the penalty time has passed
        if ((frozen || slowed) && Time.time - penaltyStartTime >= penaltyLength)
        {
            EndSlow();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void AnimateMove()
    {
        // Flip player when moving left right
        if (horizontalInput > 0.01f) // the player is moving right
        {
            // changing the x value of the player's scale to a positive value makes them face right
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 

        }

        else if (horizontalInput < -0.01f) // the player is moving left
        {
            // changing the x value of the player's scale to a negative value makes them face left
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }

        // Set animator parameters
        // Animator parameters control the animation by triggering animations when the parameter is set to a certain value
            // in this case: run = true will tell the animator that the character is running
            // syntax: anim.SetBool("paramName", value)
            // here we are using the value of horizontalInput to assign a true or false value to the bool "run"
                // horizontalInput = 0 when none of the arrow keys are pressed
                // i.e. run is true when the player is moving/arrow keys are being pressed
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
        anim.SetBool("sliding", sliding);
        anim.SetBool("holdingObject", holdingFood); // || holdingTicket
        // anim.SetBool("swing", swinging);
    }

    private bool isGrounded()
    {
        // check if the player is grounded or not by using a boxcast to check the space under the player

        // make the boxcast a sliver that's the same width as the player's collider
        Vector2 offset = new Vector2(coll.bounds.center.x, coll.bounds.center.y - coll.bounds.size.y * 0.5f);
        Vector2 sliver = new Vector2(coll.bounds.size.x, 0.2f);

        RaycastHit2D raycastHit = Physics2D.BoxCast(offset, sliver, 0, Vector2.down, 0.2f, FloorLayer);

        return raycastHit.collider != null;
    }

    // laying the groundwork for switching over from two raycasts to one
    private GameObject hitObject()
    {
        Vector2 raisedCenter = new Vector2(coll.bounds.center.x, coll.bounds.center.y + 0.1f);
        Vector2 size = new Vector2(coll.bounds.size.x, coll.bounds.size.y - 0.1f);
        RaycastHit2D raycastHit = Physics2D.BoxCast(raisedCenter, size, 0, Vector2.right, 0.1f);

        GameObject obj = raycastHit.collider.gameObject;

        if (obj != body.gameObject)
        {
            return obj;
        }
        
        return null;
    }

    // reset to original settings for testing
    private void Reset()
    {
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        transform.position = new Vector2(-8, 4);
        speed = originalSpeed;
        jumpPower = originalJumpPower;
        slowed = false;
        hinge.enabled = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
        body.freezeRotation = true;
        body.mass = 1f;
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        anim.SetTrigger("jump"); // trigger the jump animation
    }

    private void Slide()
    {
        anim.SetTrigger("slide");
        sliding = true;
        // later: disable direction changing?
    }

    private void EndSlide()
    {
        coll.size = new Vector2(1f, 2.6f);
        coll.offset = new Vector2(0f, -0.96f);
        sliding = false;

        // later: re enable direction changing
    }

    private void Swing(GameObject chandelier)
    {
        hinge.enabled = true;

        body.mass = 0.0001f;

        BoxCollider2D cColl = chandelier.GetComponent<BoxCollider2D>(); // the chandelier's box collider
        hinge.connectedBody = chandelier.GetComponent<Rigidbody2D>(); // connect this chandelier's instance to penwing's hinge


        // needed for later
        // the point the sprite rotates around relative to itself in local space
        // hinge.anchor = new Vector2(1.3f, -0.4f);

        // // connected anchor = the point relative to the connected item (in this case, the chandelier)
        // // 0, 0 = the center of the chandelier
        // // so the relative x is the center, i.e. 0
        // float aX = 0;
        // // and the relative y is the center (0) minus half the height of the chandelier
        // float aY = cColl.bounds.size.y * -0.5f;
        // hinge.connectedAnchor = new Vector2(aX, aY);
        
        // move penwing so they are positioned at the middle of the bottom of the chandelier.
        // the additional modifiers using penwing's size are done because transform.position places penwing's center point at the given coordinates, which we need to account for
        // x = the x-value of the chandelier's center
        float pX = cColl.bounds.center.x + coll.bounds.size.x * 0.5f;
        // y = the y-value of the chandelier's center minus half the height of the chandelier minus half the height of penwing
        float pY = cColl.bounds.center.y - cColl.bounds.size.y * 0.5f - coll.bounds.size.y * 0.5f;
        transform.position = new Vector2(pX, pY);

        // unfreeze rotation
        body.freezeRotation = false;

        body.angularVelocity = speed;

        swinging = true;
    }

    private void StopSwinging(GameObject chandelier)
    {
        hinge.enabled = false;

        body.mass = 1f;

        chandelierObject = null;

        // re-freeze rotations
        transform.eulerAngles = new Vector3(0, 0, 0);
        body.freezeRotation = true;

        swinging = false;
    }

    private void PickUpFood(RaycastHit2D hitInfo)
    {
        foodObject = hitInfo.collider.gameObject;
        foodObject.GetComponent<Rigidbody2D>().isKinematic = true;
        foodObject.transform.position = grabPoint.position;
        foodObject.transform.SetParent(transform);
        holdingFood = true;
    }

    // player drops the food if they were holding any
    private void DropFood(RaycastHit2D HitInfo)
    {
        if (holdingFood)
        {
            holdingFood = false;
            foodObject.GetComponent<Rigidbody2D>().isKinematic = false;
            foodObject.transform.SetParent(null);
            foodObject = null;
        }
    }

    // player is slowed for x seconds
    private void Slow()
    {
        // if the player is not already slowed
        if (!slowed)
        {
            penaltyStartTime = Time.time;
            anim.speed /= 3;
            slowed = true;

            // speed = 2f;
            // jumpPower = 2f;
            body.drag = 50f;

            // snailRenderer.enabled = true; // turn on snail animation
        }
    }

    // remove any debuffs
    private void EndSlow()
    {
        if (slowed)
        {
            anim.speed *= 3;
            slowed = false;
            // speed = originalSpeed;
            // jumpPower = originalJumpPower;
            body.drag = 0f;
            // snailRenderer.enabled = false; // snail animation off
        }
    }
}
