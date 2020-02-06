using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpSpeed = 8.0f; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currPos = transform.position;
        currPos.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        transform.position = currPos; 

        // jump button tbt, currently default 
        if (Input.GetButton("Jump"))
        {
            // assuming ball is rigidbody 
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpSpeed); 
        }

    }
}
