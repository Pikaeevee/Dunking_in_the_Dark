﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyPlatformScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // be able to detect multiple collisions 
    // set player's onIce variable to true/false 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<BallMovement>().onIce = true; 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<BallMovement>().onIce = false;
        }
    }
}
