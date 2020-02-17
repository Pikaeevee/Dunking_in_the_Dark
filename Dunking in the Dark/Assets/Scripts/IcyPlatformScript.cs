using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyPlatformScript : MonoBehaviour
{
    public AudioSource powerupSFX;
    public float timeTillNoiseStop = .5f;

    private float timePlayerOff;
    
    // Start is called before the first frame update
    void Start()
    {
        powerupSFX = GetComponent<AudioSource>();
        powerupSFX.loop = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (timePlayerOff > 0)
        {
            timePlayerOff -= Time.deltaTime;
            if (timePlayerOff <= 0)
            {
                timePlayerOff = 0;
                powerupSFX.Stop();
            }
        }
        
    }

    // be able to detect multiple collisions 
    // set player's onIce variable to true/false 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
        {
            if (!powerupSFX.isPlaying)
            {
                powerupSFX.Play();
            }
            Debug.Log("player on ice");
            collision.gameObject.GetComponent<BallMovement>().StartIce(); 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
        {
            //powerupSFX.Stop();
            timePlayerOff = timeTillNoiseStop;
            collision.gameObject.GetComponent<BallMovement>().StopIce();
        }
    }
}
