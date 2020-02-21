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

    private MatchPlayers darkness;
    private int registeredIndex;
    public float darknessSize = 3;
    public float darknessChangeTime = 1;
    

    void Start()
    {
        powerupSFX = Camera.main.GetComponent<AudioSource>(); //GetComponent<AudioSource>();
        powerupSFX.volume = 0.5f;
        if (duration > respawnTime)
        {
            Debug.LogError("Weirdness in powerup, it's respawning before it has finished");
        }
        
        darkness = GameManager.FindObjectOfType<MatchPlayers>();
        StartCoroutine(setupObject());
    }
    
    IEnumerator setupObject()
    {
        yield return new WaitForSeconds(.1f);
        //Register
        registeredIndex = darkness.Register(this.gameObject);
        print("Attempting to register powerup. Id is " + registeredIndex);
        StartCoroutine(LightsUp(darknessChangeTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator LightsDown(float time)
    {
        float timer = 0;
        yield return null;
        while (timer < time)
        {
            timer += Time.deltaTime;
            float lerpAmount = timer / time;
            if (lerpAmount > 1)
            {
                lerpAmount = 1;
            }
            float amount = Mathf.Lerp(darknessSize,0, lerpAmount);
            darkness.setPowerupSize(registeredIndex, amount);
            yield return null;
        }
    }
    
    IEnumerator LightsUp(float time)
    {
        float timer = 0;
        yield return null;
        while (timer < time)
        {
            timer += Time.deltaTime;
            float lerpAmount = timer / time;
            if (lerpAmount > 1)
            {
                lerpAmount = 1;
            }
            float amount = Mathf.Lerp(0, darknessSize, lerpAmount);
            darkness.setPowerupSize(registeredIndex, amount);
            yield return null;
        }
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
        StartCoroutine(LightsDown(darknessChangeTime));
        yield return new WaitForSeconds(respawnTime);
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<Renderer>().enabled = true;
        StartCoroutine(LightsUp(darknessChangeTime));
    }
}
