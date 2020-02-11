using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSpikeScript : MonoBehaviour
{
    // Gives the player who touches it big, spikey spikes
    
    public float duration = 8.0f;

    private GameObject player;
    private AudioSource powerupSFX;
    public AudioClip spikeNoise;

    void Start()
    {
        powerupSFX = Camera.main.GetComponent<AudioSource>(); //GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            powerupSFX.PlayOneShot(spikeNoise);
            //Hide this objects
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            gameObject.GetComponent<Renderer>().enabled = false;

            player = collision.gameObject;



            StartEffect();
        }
    }

    //This one doesn't need to be a coroutine, player handles this internally.
    private void StartEffect()
    {
        player.GetComponent<BallMovement>().setSpikey(duration);
        Destroy(this.gameObject); 
    }
}
