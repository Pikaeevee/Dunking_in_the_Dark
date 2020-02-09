using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class BallMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpSpeed = 8.0f;
    public float stickySpeed = 0.5f; // multiplier to normal velocity

    public bool onIce = false;
    public bool onSticky = false; 

    private Rigidbody2D rb;

    private float velocity;
    Vector2 movement; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currPos = transform.position;
        velocity = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

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
            // assuming ball is rigidbody 
            Jump(); 
        }

    }

    private void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>(); 
        Debug.Log(movement);

        float horiz = movement.x;

        velocity = horiz * speed * Time.deltaTime; 
    }

    private void OnJump()
    {
        Jump(); 
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpSpeed);
    }
}
