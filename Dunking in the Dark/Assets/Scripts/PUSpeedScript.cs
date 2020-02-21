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
    
    

    private MatchPlayers darkness;
    private int registeredIndex;
    public float darknessSize = 3;
    public float darknessChangeTime = 1;

    void Start()
    {
        powerupSFX = Camera.main.GetComponent<AudioSource>(); //GetComponent<AudioSource>();
        if (duration > respawnTime)
        {
            Debug.LogError("Weirdness in powerup, it's respawning before it has finished");
        }

        darkness = GameManager.FindObjectOfType<MatchPlayers>();
        StartCoroutine(setupObject());
    }

    //Setup coroutine to avoid weirdness with registering
    IEnumerator setupObject()
    {
        yield return new WaitForSeconds(.1f);
        //Register
        registeredIndex = darkness.Register(this.gameObject);
        print("Attempting to register powerup. Id is " + registeredIndex);
        if (registeredIndex == -1)
        {
            Debug.LogError("Powerup was unable to correctly register with matchplayers. This is bad. Yell at Woody.");
        }
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
            if (powerupNoise.name == "slowdown") { powerupSFX.PlayOneShot(powerupNoise, 0.2f); }
            else { powerupSFX.PlayOneShot(powerupNoise); }
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
        StartCoroutine(LightsDown(darknessChangeTime));
        yield return new WaitForSeconds(duration);
        
        

        ball.speedMultiplier = ball.speedMultiplier / speedMultiplier;
        
        yield return new WaitForSeconds(respawnTime - duration);
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<Renderer>().enabled = true;
        StartCoroutine(LightsUp(darknessChangeTime));
        
    }
}
