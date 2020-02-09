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

    private GameObject player; 
    private float ogSpeed; 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
        {
            GetComponent<SpriteRenderer>().enabled = false;// disable powerup 

            if (!enemyDebuff)
            {
                player = collision.gameObject;
                ogSpeed = player.GetComponent<BallMovement>().speed;
            }
            // else debuff TODO
            // find other player(s)
            // set 'player' var to chosen player 
            // get ogspeed of player 

            StartCoroutine(StartEffect()); 
        }
    }

    IEnumerator StartEffect()
    {
        player.GetComponent<BallMovement>().speed *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        player.GetComponent<BallMovement>().speed = ogSpeed;

        Destroy(this.gameObject); 
    }
}
