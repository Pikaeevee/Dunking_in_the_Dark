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

    Vector2 movement;

    private float velocity; 

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
        //velocity = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        // check for different platforms 
        if (onSticky)
        {
            velocity *= stickySpeed; // slows down 
        }

        if (onIce)
        {
            rb.AddForce(new Vector2(velocity, currPos.y));
        }
        else
        {
            transform.position = new Vector2(currPos.x + velocity, currPos.y);
        }


        // jump button tbt, currently default 
        if (Input.GetButton("Jump"))
        {
            if (jumpcooldown <= 0)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        print("Attempting to Jump!");
        //Raycast to make sure we can jump
        RaycastHit2D results;
        LayerMask mask = LayerMask.GetMask("Default");
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(howCloseToJump.x, howCloseToJump.y, 0));
    }
}
