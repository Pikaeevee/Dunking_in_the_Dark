using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpSpeed = 8.0f;
    [SerializeField] private Vector2 howCloseToJump;
    public float stickySpeed = 0.5f; // multiplier to normal velocity

    public bool onIce = false;
    public bool onSticky = false;
    [SerializeField] private float jumpWait = .05f;
    private float jumpcooldown = 0;

    private Rigidbody2D rb;

    public KeyCode jumpKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    public string jumpButton = "Jump_P1";
    public string horizAxis = "Horizontal_P1";

    Vector2 movement;

    private float velocity;

    //Float that says how much longer our powerup lasts
    private float spikeyTime;
    private bool isSpikey;
    
    //Powerup Modifiers
    public float speedMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (gameObject.layer != 9)
        {
            Debug.LogError("Players should be in the Player layer!");
            Debug.Log("Moving the players to the correct layer");
            gameObject.layer = 9;
        }
    }

    // Update is called once per frame
    void Update()
    {
        jumpcooldown -= Time.deltaTime;
        Vector2 currPos = transform.position;

        /*
        if (Input.GetKey(leftKey))
        {
            velocity = -1 * speed * speedMultiplier;//* Time.deltaTime; 
        }
        else if (Input.GetKey(rightKey))
        {
            velocity = speed * speedMultiplier;// * Time.deltaTime; 
        }
        else
        {
            //Base case so that ball doesn't just move forever
            velocity = 0;
        }
        */

        velocity = Input.GetAxis(horizAxis) * speed * speedMultiplier;

        // check for different platforms 
        if (onSticky)
        {
            velocity *= stickySpeed; // slows down 
        }

        if (onIce)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(velocity, rb.velocity.y),  0.01f);
            //rb.AddForce(new Vector2(velocity, currPos.y));
        }
        else
        {
            //Moving over to a rigidbody system
            rb.velocity = new Vector2(velocity, rb.velocity.y);
            //transform.position = new Vector2(currPos.x + velocity, currPos.y);
        }


        // jump button tbt, currently default 
        if (Input.GetButtonDown(jumpButton))
        {
            if (jumpcooldown <= 0)
            {
                Jump();
            }
        }
        
        //Update spike factor
        if (spikeyTime > 0)
        {
            //Countdown
            spikeyTime -= Time.deltaTime;
            if (spikeyTime <= 0)
            {
                //We need to reset!
                undoSpikey();
            }
        }
    }

    //TODO: Make these do something more interesing than just scale
    public void setSpikey(float duration)
    {
        if (spikeyTime <= 0)
        {
            isSpikey = true;
            howCloseToJump *= 2;
            transform.localScale = transform.localScale * 1.2f;
        }

        spikeyTime += duration;
        
    }

    public void undoSpikey()
    {
        spikeyTime = 0;
        isSpikey = false;
        howCloseToJump /= 2;
        transform.localScale = transform.localScale / 1.2f;
    }

    private void Jump()
    {
        //print("Attempting to Jump!");
        //Raycast to make sure we can jump
        RaycastHit2D results;
        LayerMask mask = LayerMask.GetMask("Ground");
        results = Physics2D.Raycast(transform.position, howCloseToJump.normalized, howCloseToJump.magnitude, mask);
        if (results.collider)
        {
            //print("We correctly collided!");
            //We hit something!
            // assuming ball is rigidbody
            rb.AddForce(Vector2.up * jumpSpeed);
            jumpcooldown = jumpWait;
        }
        else
        {
            //print("No collider hit!");
        }
    }

    private void OnMove(InputValue value)
    {
        Debug.Log("moving with stick"); 
        movement = value.Get<Vector2>();

        float horiz = movement.x;

        velocity = horiz * speed * Time.deltaTime;
    }

    private void OnJump()
    {
        Debug.Log("jumped through controller"); 
        if (jumpcooldown <= 0)
        {
            Jump(); 
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if (isSpikey)
            {
                //Pop the other player!
                print("Popped you!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(howCloseToJump.x, howCloseToJump.y, 0));
    }
}
