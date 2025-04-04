using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

using UnityEngine.InputSystem;

using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Dynamic;
using System;
using System.Net.Http;

public class PlayerMovement : MonoBehaviour
{
    private GameObject self;
    private Rigidbody2D body;
    
    // variables for animation
    private Animator anim;
    private SpriteRenderer rend;
    private bool sliding;
    private LayerMask floorMask;
    private Vector2 lookDirection;

    // variables for player
    private float playerXSize, playerYSize;
    [SerializeField] private float waitRespawn;

    // elevator vars
    private LayerMask elevatorMask;
    private bool onElevator;
    private GameObject elevator;
    private LayerMask sensorMask;


    // movement variables
    private float horizontalInput;
    [SerializeField] private float speed; // seralizeField makes it editable from unity ui
    private float originalSpeed;
    [SerializeField] private float jumpPower;
    private float originalJumpPower;


    // hitObject
    private float sign;


    // sliding variables
    private BoxCollider2D coll;

    // swinging variables
    private bool swinging;
    // [SerializeField] private Transform upperRayPoint;
    // [SerializeField] private float upperRayDistance;
    private int chandelierLayer;
    private LayerMask chandelierMask;
    private GameObject chandelierObject;
    private HingeJoint2D hinge;


    // penalty time length variables
    [SerializeField] private float penaltyLength;
    private float penaltyStartTime;


    // penalty variables
    private bool slowed = false;
    private int currentFloor; // tracks what floor the player is currently on for the falling penalty
    private int lastFloor;
    private bool usedElevator = false;
    private bool grounded;
    private bool wasGroundedLastUpdate = false;


    // Variables for grabbing
    private bool holdingFood = false;
    [SerializeField] private Transform grabPoint;
    // [SerializeField] private Transform lowerRayPoint;
    // [SerializeField] private float lowerRayDistance;
    private GameObject foodObject;
    private int foodLayer;
    private LayerMask foodMask;


    // Variables for taking and placing orders
    private LayerMask tableMask;
    private int tableLayer;
    private GameObject table;
    // private int tableState;
    private int kitchenLayer;


    public Tutorial tutorial;

    private void Awake()
    {
        self = transform.gameObject;
        // get references from Player object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        hinge = GetComponent<HingeJoint2D>();

        // elevator
        elevator = GameObject.Find("Elevator");

        // to be able to return to original speed and jumpPower after penalty effects
        originalSpeed = speed;
        originalJumpPower = jumpPower;
    }

    private void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
        elevatorMask = LayerMask.GetMask("Elevator");
        sensorMask = LayerMask.GetMask("Sensor Plate");
        chandelierMask = LayerMask.GetMask("Chandelier");
        foodMask = LayerMask.GetMask("Food");
        tableMask = LayerMask.GetMask("Table");

        foodLayer = LayerMask.NameToLayer("Food");
        chandelierLayer = LayerMask.NameToLayer("Chandelier");
        tableLayer = LayerMask.NameToLayer("Table");
        kitchenLayer = LayerMask.NameToLayer("Kitchen");

        playerXSize = transform.localScale.x;
        playerYSize = transform.localScale.y;
    }

    // private void FixedUpdate()
    // {
    //     grounded = IsGrounded()
        
    // }

    
    private void Update()
    {
        grounded = IsGrounded();
        onElevator = IsOnElevator();

        if (grounded)
        {
            currentFloor = GetFloorNum();
        }

        if (onElevator)
        {
            // make the current floor = 99 so that the falling penalty works properly
            currentFloor = 99;
        }

        // for moving
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        lookDirection = new Vector2(horizontalInput, 0f);
        

        AnimateMove();

        if (transform.position.y <= -6f)
        {
            Reset();
        }

        // elevator logic /////////////////////////
        if (IsOnElevator()) // && !grounded
        {
            // my attempts to get the player to follow the elevator more closely
            // Transform eleTransform = elevator.transform;
            // float invElevatorXScale = 1 / eleTransform.localScale.x;
            // float invElevatorYScale = 1 / eleTransform.localScale.y;

            // transform.SetParent(eleTransform);
            // transform.localScale = new Vector2(invElevatorXScale, invElevatorYScale);
            // transform.localScale = originalScale;
            // elevatorPosition = new Vector2(body.position.x, elevatorPosition.y);
            // body.position = Vector2.MoveTowards(body.position, elevatorPosition, Time.deltaTime * 2);

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                elevator.SendMessage("Up");
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                elevator.SendMessage("Down");
            }
        }

        // if the player is on the sensor plate, call the elevator to their floor
        if (IsOnSensor())
        {
            string name = IsOnSensor().name;
            elevator.SendMessage("ToSpecificFloor", name);
        }

        // more attempts to get the player to follow the elevator
        // else
        // {
        //     transform.SetParent(null);
        //     transform.localScale = originalScale;
        // }


        // game logic /////////////////////////////////
        // x to jump
        if (Input.GetKeyDown(KeyCode.X) && (grounded || onElevator) && !swinging) // key down prevents them from holding down the button and floating
        {
            Jump();
        }

        // sliding
        if (Input.GetKeyDown(KeyCode.DownArrow) && IsGrounded())
        {
            Slide();
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) || !IsGrounded())
        {
            EndSlide();
        }

        // drop food if the player is holding any
        // eliminates error cases where the boxcast can't find the food for any reason
        if (holdingFood && (IsGrounded() || IsOnElevator()) && Input.GetKeyDown(KeyCode.Z))
        {
            Invoke(nameof(DropFood), 0f);
        }

        // look for collision with interactable objects, namely:
        // food, chandelier, tables
        if (hitObject())
        {
            GameObject obj = hitObject();
            int layer = obj.layer;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if ((IsGrounded() || IsOnElevator()) && layer == foodLayer)
                {
                    if (!foodObject)
                    {
                        PickUpFood(obj);
                    }
                    
                    // else
                    // {
                    //     Debug.Log(hitObject());
                    //     DropFood();
                    // }
                }

                if (layer == chandelierLayer && !IsGrounded())
                {
                    Swing(obj);
                }

                if (layer == tableLayer)
                {
                    Table table = obj.GetComponent<Table>();
                    if (table)
                    {
                        switch (table.State)
                        {
                            // customer is ready to order
                            case 1:
                            {
                                table.TakeOrder();
                                break;
                            }

                            // customer is waiting for order
                            case 2:
                            {
                                // only trigger delivery if we have food and
                                // the table does not already have food
                                if (holdingFood && table.transform.childCount == 0)
                                {
                                    DeliverOrder(table);
                                }

                                break;
                            }

                            default:
                            {
                                break;
                            }
                        }
                    }
                }
            }         
        }

        // if (Input.GetKeyUp(KeyCode.Z) || IsGrounded())
        // {
        //     StopSwinging(chandelierObject);
        // }

        // if the player is swinging
        if (swinging)
        {
            // release z to unswing
            if (Input.GetKeyUp(KeyCode.Z) || IsGrounded())
            {            
                // store the angular velocity before stopping to use for propulsion
                float currentAngularVelocity = body.angularVelocity;
                // Vector2 currentVelocity = body.velocity;
                
                StopSwinging(chandelierObject);
                
                // propel penwing after stopping swing
                Propel(currentAngularVelocity);
            }
        }


        // penalties //////////////////////////////////////////////////////////////
        
        // issue food drop penalty if they fell to the next floor
        // do this by checking 3 things:
        // 1. !wasGrounded and grounded or onElevator: checks that they were not grounded last frame, and are now grounded/on the elevator, signifying a landing of some sort
        // 2. lastFloor != currentFloor: they've changed floors, meaning the landing was not from a successful jump or swing
        // 3. !usedElevator: they didn't use the elevator to change floors
        // the fourth check, lastFloor == 0, checks if it was the start of the game, which is the only case when lastFloor = 0
        if ((!wasGroundedLastUpdate && (grounded || onElevator) && lastFloor != currentFloor && !usedElevator) || lastFloor == 0)
        {
            DropFood();

            StartCoroutine(Blink());
        }

        // set usedElevator back to false once they step onto the floor again
        if (IsGrounded() && usedElevator)
        {
            usedElevator = false;
        }

        // if the player is slowed, check if the penalty time for the slow has passed
        if (slowed && Time.time - penaltyStartTime >= penaltyLength)
        {
            EndSlow();
        }

        // for testing: reset the player's position and all settings
        if (Input.GetKey(KeyCode.R))
        {
            Reset();
        }

        // if (hitObject())
        // {
        //     Debug.Log(hitObject().name);
        // }

        // else
        // {
        //     Debug.Log("null");
        // }

        // update the information from this frame
        wasGroundedLastUpdate = grounded;
        lastFloor = currentFloor;
    }
    
    // void OnDrawGizmos()
    // {
    //     if (Application.isPlaying)
    //     {
            
    //         float maxDistance = 0.2f; // Adjust as needed

    //         sign = MathF.Sign(horizontalInput);

    //         // if they are not moving, leave it where it is
    //         if (sign == 0)
    //         {
    //             if (transform.localScale.x > 0)
    //             {
    //                 sign = 1;
    //             }

    //             else if (transform.localScale.x < 0)
    //             {
    //                 sign = -1;
    //             }
    //         }

    //         // make the boxcast
    //         // position = the side of the player, multiplied by sign
    //         float xPos = coll.bounds.center.x + coll.bounds.size.x * sign;
    //         float yPos = coll.bounds.center.y + 0.1f;
    //         Vector2 position = new Vector2(xPos, yPos);

    //         // size = a sliver that's the nearly the same height as the player's collider
    //         Vector2 sliver = new Vector2(0.2f, coll.bounds.size.y * 0.9f);

    //         // combine the masks that this box cast looks for
    //         int combinedMask = foodMask | chandelierMask | tableMask;

    //         RaycastHit2D hit = Physics2D.BoxCast(position, sliver, 0, lookDirection, 0.2f, combinedMask);
            
    //         // Set color based on hit
    //         Gizmos.color = hit.collider != null ? Color.green : Color.red;
            
    //         // Draw the starting box
    //         Gizmos.DrawWireCube(position, sliver);
            
    //         // Draw the ray
    //         Gizmos.DrawRay(position, lookDirection * (hit.collider != null ? hit.distance : maxDistance));
            
    //         // Draw the ending box
    //         Vector2 endPos = position + lookDirection * (hit.collider != null ? hit.distance : maxDistance);
    //         Gizmos.DrawWireCube(endPos, sliver);
    //     }
    // }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        
        // Draw ground check
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Vector2 position = new Vector2(coll.bounds.center.x, coll.bounds.center.y - coll.bounds.size.y * 0.5f);
        Vector2 sliver = new Vector2(coll.bounds.size.x, 0.2f);
        Gizmos.DrawWireCube(position, sliver);
        Gizmos.DrawLine(position, position + Vector2.down * 0.2f);
    }

    // REDUCES THE PHASING THROUGH FLOOR GLITCH BY A LOT WITH THE TRADE OFF
    // THAT YOU FALL BETWEEN THE FLOOR AND ELEVATOR ON GROUND FLOOR AND FLOOR 5
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (floorMask == (floorMask | (1 << collision.gameObject.layer)))
    //     {
    //         Debug.Log("Floor collision detected");
            
    //         // force position adjustment
    //         Vector2 contactPoint = collision.GetContact(0).point;
    //         float newY = contactPoint.y + coll.bounds.extents.y + 0.01f;
    //         transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    //     }
    // }

 
    private int GetFloorNum()
    {
        Vector2 position = new Vector2(coll.bounds.center.x, coll.bounds.center.y - coll.bounds.size.y * 0.5f);
        Vector2 sliver = new Vector2(coll.bounds.size.x, 0.2f);

        RaycastHit2D raycastHit = Physics2D.BoxCast(position, sliver, 0, Vector2.down, 0.2f, floorMask);

        int num = 1;

        string name = raycastHit.collider.gameObject.name;

        // temp hard coded
        if (name.Contains("Ground"))
        {
            num = 1;
        }

        else if (name.Contains("2"))
        {
            num = 2;
        }

        else if (name.Contains("3"))
        {
            num = 3;
        }

        else if (name.Contains("4"))
        {
            num = 4;
        }

        else if (name.Contains("5"))
        {
            num = 5;
        }

        return num;
    }

    private void AnimateMove()
    {
        // Flip player when moving left right
        if (horizontalInput > 0.01f) // the player is moving right
        {
            // changing the x value of the player's scale to a positive value makes them face right
            transform.localScale = new Vector3(playerXSize, playerYSize, 0.5f); 

        }

        else if (horizontalInput < -0.01f) // the player is moving left
        {
            // changing the x value of the player's scale to a negative value makes them face left
            transform.localScale = new Vector3(-playerXSize, playerYSize, 0.5f);
        }

        // Set animator parameters
        // Animator parameters control the animation by triggering animations when the parameter is set to a certain value
            // in this case: run = true will tell the animator that the character is running
            // syntax: anim.SetBool("paramName", value)
            // here we are using the value of horizontalInput to assign a true or false value to the bool "run"
                // horizontalInput = 0 when none of the arrow keys are pressed
                // i.e. run is true when the player is moving/arrow keys are being pressed
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", IsGrounded());
        anim.SetBool("sliding", sliding);
        anim.SetBool("holdingObject", holdingFood); // || holdingTicket
        anim.SetBool("swinging", swinging);
    }

    // use box cast to check when the player is on the floor
    private bool IsGrounded()
    {
        // check if the player is grounded or not by using a boxcast to check the space under the player
 
        // make the boxcast a sliver that's the same width as the player's collider
        Vector2 position = new Vector2(coll.bounds.center.x, coll.bounds.center.y - coll.bounds.size.y * 0.5f);
        Vector2 sliver = new Vector2(coll.bounds.size.x, 0.2f);

        RaycastHit2D groundRayCast = Physics2D.BoxCast(position, sliver, 0, Vector2.down, 0.2f, floorMask);

        return groundRayCast.collider != null;
    }

    // use box cast to check when the player is on the elevator
    private bool IsOnElevator()
    {
        // check if the player is grounded or not by using a boxcast to check the space under the player
 
        // make the boxcast a sliver that's the same width as the player's collider
        Vector2 position = new Vector2(coll.bounds.center.x, coll.bounds.center.y - coll.bounds.size.y * 0.5f);
        Vector2 sliver = new Vector2(coll.bounds.size.x, 0.2f);

        RaycastHit2D elevatorRayCast = Physics2D.BoxCast(position, sliver, 0, Vector2.down, 0.2f, elevatorMask);

        return elevatorRayCast.collider != null;
    }

    // use box cast to check when the player is on the sensor to call the elevator
    private GameObject IsOnSensor()
    {
        // check if the player is on the sensor plate or not by using a boxcast to check the space under the player
 
        // make the boxcast a sliver that's the same width as the player's collider
        Vector2 position = new Vector2(coll.bounds.center.x, coll.bounds.center.y - coll.bounds.size.y * 0.5f);
        Vector2 sliver = new Vector2(coll.bounds.size.x, 0.2f);

        RaycastHit2D sensorRayCast = Physics2D.BoxCast(position, sliver, 0, Vector2.down, 0.2f, sensorMask);

        // return sensorRayCast.collider != null;
        if (sensorRayCast.collider)
        {
            return sensorRayCast.collider.gameObject;
        }

        return null;
        
    }

    // use box cast to check if there is an interactable object beside the player
    private GameObject hitObject()
    {
        // use sign as a multiplier to move the box cast to
        // the left or right side of the player depending on
        // which way they're moving
        
        sign = MathF.Sign(horizontalInput);

        // if they are not moving, leave it where it is
        if (sign == 0)
        {
            if (transform.localScale.x > 0)
            {
                sign = 1;
            }

            else if (transform.localScale.x < 0)
            {
                sign = -1;
            }
        }

        // make the boxcast
        // position = the side of the player, multiplied by sign
        float xPos = coll.bounds.center.x + coll.bounds.size.x * sign;
        float yPos = coll.bounds.center.y + 0.1f;
        Vector2 position = new Vector2(xPos, yPos);

        // size = a sliver that's the nearly the same height as the player's collider
        Vector2 sliver = new Vector2(0.2f, coll.bounds.size.y * 0.9f);

        // combine the masks that this box cast looks for
        int combinedMask = foodMask | chandelierMask | tableMask;

        RaycastHit2D sideInfo = Physics2D.BoxCast(position, sliver, 0, lookDirection, 0.2f, combinedMask);
        
        // if we hit something, return that object
        if (sideInfo.collider)
        {
            return sideInfo.collider.gameObject;
        }
        
        // nothing was hit, return null
        return null;
    }

    // reset to original settings for testing
    private void Reset()
    {
        StartCoroutine(WaitBeforeReset());
    }

    IEnumerator WaitBeforeReset()
    {
        yield return new WaitForSeconds(waitRespawn);
        body.velocity = Vector2.zero;
        transform.position = new Vector2(-0, -4);
        speed = originalSpeed;
        jumpPower = originalJumpPower;
        slowed = false;
        hinge.enabled = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
        body.freezeRotation = true;
        body.mass = 1f;
    }

    ///////////////////////////////////////////////////////////////////////////////////
    // METHODS FOR ELEVATOR ///////////////////////////////////////////////////////////
    
    // called in updownchunk.cs
    private void FollowElevator(Vector3 target)
    {
        // elevatorPosition = target;
        // transform.SetParent(elevator.transform);
        usedElevator = true;
    }

    // JUMP ///////////////////////////////////////////

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        anim.SetTrigger("jump"); // trigger the jump animation
        // body.velocity = new Vector2(body.velocity.x, body.velocity.y);
    }

    ///////////////////////////////////////////////////////////////////////////////////
    // METHODS FOR SLIDING ////////////////////////////////////////////////////////////////////////////////////////
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

    ///////////////////////////////////////////////////////////////////////////////////
    // METHODS FOR SWINGING ////////////////////////////////////////////////////////////////////////////////////////
    private void Swing(GameObject chandelier)
    {
        anim.SetTrigger("Swing");

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

    private void Propel(float angularVelocity)
    {
        // calculate the direction based on the angular velocity
        float direction = Mathf.Sign(angularVelocity);
        
        // disable collisions between player and food while propelling
        if (holdingFood && foodObject != null)
        {
            // temporarily disable the food's collider
            Collider2D foodColl = foodObject.GetComponent<Collider2D>();

            if (foodColl != null)
            {
                foodColl.enabled = false;
            }
        }
        
        // change the velocity
        float xForce = 10f;
        float yForce = 5f;
        body.velocity = new Vector2(direction * xForce, yForce);
        
        // re-enable collisions after a short delay
        if (holdingFood && foodObject != null)
        {
            StartCoroutine(EnableFoodCollision());
        }
    }

    private IEnumerator EnableFoodCollision()
    {
        // wait for propulsion to complete before re-enabling the collider
        yield return new WaitForSeconds(0.5f);
        
        if (foodObject != null)
        {
            Collider2D foodColl = foodObject.GetComponent<Collider2D>();
            
            if (foodColl != null)
            {
                foodColl.enabled = true;
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////
    // METHODS FOR PICKING UP AND PUTTING DOWN FOOD ////////////////////////////////////////////////////////////////////////////////////////
    private void PickUpFood(GameObject obj)
    {
        foodObject = obj;

        Collider2D foodColl = foodObject.GetComponent<Collider2D>();
        foodColl.isTrigger = false;

        Rigidbody2D foodRb = foodObject.GetComponent<Rigidbody2D>();
        foodRb.isKinematic = true;
        foodRb.simulated = false;
        // foodRb.mass = 0.0001f;

        Transform foodTr = foodObject.GetComponent<Transform>();
        foodTr.position = grabPoint.position;
        foodTr.SetParent(transform);
        
        holdingFood = true;

        if (tutorial)
        {
            tutorial.OrderPickedUp();
        }
    }

    // player drops the food if they were holding any
    private void DropFood()
    {
        if (holdingFood && foodObject)
        {
            Rigidbody2D foodRb = foodObject.GetComponent<Rigidbody2D>();
            foodRb.simulated = true;
            foodRb.bodyType = RigidbodyType2D.Dynamic;

            Transform foodTr = foodObject.GetComponent<Transform>();
            foodTr.SetParent(null);            

            foodObject = null;
            holdingFood = false;
        }
    }

    // blink as a visual indicator after falling and dropping food
    private IEnumerator Blink()
    {
        for (int i = 0; i < 4; i++)
        {
            if (rend.enabled)
            {
                rend.enabled = false;
            }

            else
            {
                rend.enabled = true;
            }

            yield return new WaitForSeconds(0.1f);
        }

        rend.enabled = true;
    }        

    ///////////////////////////////////////////////////////////////////////////////////
    // METHODS FOR SLOWS ////////////////////////////////////////////////////////////////////////////////////////
    // player is slowed for x seconds

    // called in WebBehaviour.cs
    private void Slow()
    {
        // if the player is not already slowed
        if (!slowed)
        {
            penaltyStartTime = Time.time;
            anim.speed /= 4;
            slowed = true;

            // speed = 2f;
            // jumpPower = 2f;
            // body.drag = 100f;
            speed /= 2;
            jumpPower /= 2;

            // snailRenderer.enabled = true; // turn on snail animation
        }
    }

    // remove any debuffs
    private void EndSlow()
    {
        if (slowed)
        {
            anim.speed *= 4;
            slowed = false;
            // speed = originalSpeed;
            // jumpPower = originalJumpPower;
            // body.drag = 0f;
            speed *= 2;
            jumpPower *= 2;
            // snailRenderer.enabled = false; // snail animation off
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////
    // ORDER AND TABLE INTERACTION METHODS ///////////////////////////////////////////////////////////////////////////////////
    // give the order to the customer
    private void DeliverOrder(Table table)
    {
        Collider2D foodColl = foodObject.GetComponent<Collider2D>();
        foodColl.isTrigger = true;

        foodObject.transform.SetParent(table.transform);

        table.DeliverOrder();
        
        foodObject = null;
        holdingFood = false;
    }
}
