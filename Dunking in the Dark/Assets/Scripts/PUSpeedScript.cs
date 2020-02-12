using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSpeedScript : MonoBehaviour
{
    // alters speed of either player who picked up powerup or another player 
    // Start is called before the first frame update

    public float speedMultiplier = 1.5f; 
    public float duration = 8.0f;
    public bool enemyDebuff = false;
    [SerializeField] private float respawnTime = 10f;

    private GameObject player;
    private AudioSource powerupSFX;
    public AudioClip powerupNoise;

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
            powerupSFX.PlayOneShot(powerupNoise);
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            gameObject.GetComponent<Renderer>().enabled = false;
            if (!enemyDebuff)
            {
                player = collision.gameObject;
                //ogSpeed = player.GetComponent<BallMovement>().speed;
            }
            else
            {
                if (collision.gameObject.CompareTag("Player1"))
                {
                    player = GameObject.FindGameObjectWithTag("Player2");
                }
                else
                {
                    player = GameObject.FindGameObjectWithTag("Player1");
                }
            }
            
            

            StartCoroutine(StartEffect()); 
        }
    }

    //Changes made here, so that the speed multipliers correctly stack and de-stack
    IEnumerator StartEffect()
    {
        BallMovement ball = player.GetComponent<BallMovement>();
        ball.speedMultiplier = ball.speedMultiplier * speedMultiplier;

        yield return new WaitForSeconds(duration);

        

        ball.speedMultiplier = ball.speedMultiplier / speedMultiplier;
        
        yield return new WaitForSeconds(respawnTime - duration);
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<Renderer>().enabled = true;
    }
}
