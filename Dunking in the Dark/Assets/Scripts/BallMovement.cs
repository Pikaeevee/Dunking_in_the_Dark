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
    private float endingIce = -1.0f;
    public bool onSticky = false;
    private float endingSticky = -1.0f;
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

    private float hasControl;

    private Sprite normalSprite;
    [SerializeField] private Sprite spikySprite;
    private Color playerCol;
    private Color tailCol;
    public float collisionSpeed;
    public float bounceReturn = 0;

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

        normalSprite = GetComponent<SpriteRenderer>().sprite;
        playerCol = GetComponent<SpriteRenderer>().color;
        tailCol = GetComponent<TrailRenderer>().startColor;
        collisionSpeed = 0;
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
        
        //Control Ice and stickiness timers
        if (endingIce > 0)
        {
            endingIce -= Time.deltaTime;
            if (endingIce < 0)
            {
                print("Turning ice off");
                onIce = false;
            }
        }
        
        if (endingSticky > 0)
        {
            endingSticky -= Time.deltaTime;
            if (endingSticky < 0)
            {
                print("Turning sticky off");
                onSticky = false;
                GetComponent<Rigidbody2D>().mass = 1;
            }
        }

        velocity = Input.GetAxis(horizAxis) * speed * speedMultiplier;
        hasControl -= Time.deltaTime;
        if (hasControl < 0)
        {
            // check for different platforms 
            if (onSticky)
            {
                if (endingSticky > 0)
                {
                    velocity = Mathf.Lerp(velocity, velocity * stickySpeed, endingSticky * 2);
                }
                else
                {
                    velocity *= stickySpeed; // slows down 
                }
            }

            if (onIce)
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(velocity, rb.velocity.y), 0.07f);
                //rb.AddForce(new Vector2(velocity, currPos.y));
            }
            else
            {
                //Moving over to a rigidbody system
                rb.velocity = new Vector2(velocity, rb.velocity.y);
                //transform.position = new Vector2(currPos.x + velocity, currPos.y);
            }
        }


        // jump button tbt, currently default 
        if (Input.GetButton(jumpButton) && hasControl <= 0)
        {
            if (jumpcooldown <= 0 && Mathf.Abs(rb.velocity.y) < speed * 1.5 * speedMultiplier)
            {
                print("Jumping!");
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
    
    //Needed for collision detection
    private void FixedUpdate()
    {
        collisionSpeed = rb.velocity.magnitude;
    }

    //TODO: Make these do something more interesing than just scale
    public void setSpikey(float duration)
    {
        if (spikeyTime <= 0)
        {
            isSpikey = true;
            howCloseToJump *= 2;
            GetComponent<SpriteRenderer>().sprite = spikySprite;
            transform.localScale = transform.localScale * 1.2f;
        }

        spikeyTime += duration;
        
    }

    public void undoSpikey()
    {
        spikeyTime = 0;
        isSpikey = false;
        howCloseToJump /= 2;
        GetComponent<SpriteRenderer>().sprite = normalSprite;
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
            rb.velocity = new Vector2(rb.velocity.x, Math.Abs(rb.velocity.y) * bounceReturn);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
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
                print("Popped other player!");
                other.gameObject.GetComponent<BallMovement>().loseControl(4f);
            }
            else
            {
                print("Knocked someone!");
                BallMovement control = other.gameObject.GetComponent<BallMovement>();
                if (collisionSpeed > control.collisionSpeed)
                {
                    //Knock them!
                    print("Knocked them!");
                    other.gameObject.GetComponent<BallMovement>().loseControl(1.5f);
                }
                else
                {
                    //Knock us!
                    print("Knocked us!");
                    loseControl(1.5f);
                    
                }
            }
        }
    }

    public void loseControl(float time)
    {
        if (hasControl > 0) return;
        if (isSpikey) return;
        rb.velocity = rb.velocity * 1.2f;
        hasControl = time;
        StartCoroutine(Blink(time, .25f));
    }

    IEnumerator Blink(float duration, float swapAmount)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        TrailRenderer trail = GetComponent<TrailRenderer>();
        bool flipped = false;
        float count = 0;
        while (count < duration)
        {
            if (flipped)
            {
                sprite.color = Color.white;
                trail.startColor = Color.white;
            }
            else
            {
                sprite.color = playerCol;
                trail.startColor = tailCol;
            }
            flipped = !flipped;
            yield return new WaitForSeconds(swapAmount);
            count += swapAmount;
        }

        sprite.color = playerCol;
        trail.startColor = tailCol;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(howCloseToJump.x, howCloseToJump.y, 0));
    }

    public void StartIce()
    {
        onIce = true;
        endingIce = -1;
    }
    public void StopIce()
    {
        endingIce = .3f;
    }

    public void StartSticky()
    {
        if (!onSticky)
        {
            GetComponent<Rigidbody2D>().mass = 1.3f;
            GetComponent<Rigidbody2D>().velocity *= new Vector2(1, 0);
        }

        onSticky = true;
        endingSticky = 0.0f;
    }
    public void StopSticky()
    {
        endingSticky = .5f;
    }

    private void OnDestroy()
    {
        print("Whomst'dve the fuck thought destroying me was a good idea");
        
    }
}
