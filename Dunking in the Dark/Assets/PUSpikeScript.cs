using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSpikeScript : MonoBehaviour
{
    // Gives the player who touches it big, spikey spikes
    
    public float duration = 8.0f;
    [SerializeField] private float respawnTime = 10f;

    private GameObject player;
    private AudioSource powerupSFX;
    public AudioClip spikeNoise;
    

    void Start()
    {
        powerupSFX = Camera.main.GetComponent<AudioSource>(); //GetComponent<AudioSource>();
        if (duration > respawnTime)
        {
            Debug.LogError("Weirdness in powerup, it's respawning before it has finished");
        }
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



            StartCoroutine(StartEffect());
        }
    }

    //This one doesn't need to be a coroutine, player handles this internally.
    IEnumerator StartEffect()
    {
        player.GetComponent<BallMovement>().setSpikey(duration);
        yield return new WaitForSeconds(respawnTime);
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<Renderer>().enabled = true;
    }
}
