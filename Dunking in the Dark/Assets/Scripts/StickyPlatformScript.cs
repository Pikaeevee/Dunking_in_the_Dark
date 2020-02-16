using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatformScript : MonoBehaviour
{
    public AudioSource powerupSFX;
    // Start is called before the first frame update
    void Start()
    {
        powerupSFX = GetComponent<AudioSource>();
        powerupSFX.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // set player's onIce variable to true/false 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
        {
            powerupSFX.Play();
            // make heavy
            //collision.gameObject.GetComponent<Rigidbody2D>().mass = 100;
            collision.gameObject.GetComponent<BallMovement>().StartSticky();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
        {
            powerupSFX.Stop();
            // revert changes 
            //collision.gameObject.GetComponent<Rigidbody2D>().mass = 1;
            collision.gameObject.GetComponent<BallMovement>().StopSticky();
        }
    }
}
