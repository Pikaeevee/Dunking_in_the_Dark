using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectScoring : MonoBehaviour
{
    //This is supplied by the main camera
    private AudioSource source;
    [SerializeField] private AudioClip scoringNoise;
    [SerializeField] private float noScoreDuration = 1f;

    [SerializeField] private Vector2 directionBallEnters;
    
    
    //Don't need to do this, unless the hoop itself has a noise
    // Start is called before the first frame update
    void Start()
    {
        //Get the noise projector on the camera
        source = Camera.main.GetComponent<AudioSource>();
        directionBallEnters = directionBallEnters.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Player1"))
        {
            //Give player one points!
            if (checkPlayerEligibility(other.gameObject))
            {
                source.PlayOneShot(scoringNoise);
                GameManager.instance.addP1();
                StartCoroutine(stopScoring());
            }
        }
        else if (other.gameObject.CompareTag("Player2"))
        {
            //Give player two points!
            if (checkPlayerEligibility(other.gameObject))
            {
                source.PlayOneShot(scoringNoise);
                GameManager.instance.addP2();
                StartCoroutine(stopScoring());
            }
        }
    }

    public bool checkPlayerEligibility(GameObject player)
    {
        Rigidbody2D rig = player.GetComponent<Rigidbody2D>();
        Vector2 velocity = rig.velocity.normalized;
        return (Vector2.Dot(velocity, directionBallEnters) > 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + directionBallEnters.x, transform.position.y + directionBallEnters.y, transform.position.z));
        Gizmos.DrawWireCube(new Vector3(transform.position.x + directionBallEnters.x, transform.position.y + directionBallEnters.y, transform.position.z), Vector3.one * .1f);
    }

    IEnumerator stopScoring()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.enabled = false;
        yield return new WaitForSeconds(noScoreDuration);
        box.enabled = true;
    }
}
