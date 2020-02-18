using System;
using System.Collections;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            other.gameObject.GetComponent<BallMovement>().StartIce();
            other.GetComponent<Rigidbody2D>().freezeRotation = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            other.gameObject.GetComponent<BallMovement>().StopIce();
            other.GetComponent<Rigidbody2D>().freezeRotation = false;
        }
    }
}
