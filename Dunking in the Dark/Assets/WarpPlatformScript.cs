using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WarpPlatformScript : MonoBehaviour
{
    private GameObject otherPlatform;

    [SerializeField] private float timeToTeleport = 1f;

    private ArrayList objectsToTeleport;
    private float timer;
    private bool canTeleport;
    private Color startColor;

    [SerializeField] private float cooldown = 3;
    [SerializeField] private float blinkTime = .5f;
    [SerializeField] private Color blinkColor;

    private ParticleSystem particles;

    private AudioSource audio;
    
    // Start is called before the first frame update
    void Start()
    {
        if (timeToTeleport <= 0)
        {
            print("Time can't be zero!");
        }
        timer = -1;
        objectsToTeleport = new ArrayList();
        Link();
        startColor = transform.parent.GetComponent<SpriteRenderer>().color;
        canTeleport = true;
        particles = GetComponent<ParticleSystem>();
        audio = GetComponent<AudioSource>();
        audio.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        //print("Timer is " + timer);
        if (timer > 0 && canTeleport)
        {
            //print("Timer subtracted");
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                print("Warping!");
                timer = -1;
                warp();
            }
        }
        
    }

    public void StartCooldown()
    {
        StartCoroutine(CoolingDown());
    }

    IEnumerator CoolingDown()
    {
        particles.Stop();
        canTeleport = false;
        bool flipped = false;
        SpriteRenderer sprite = transform.parent.GetComponent<SpriteRenderer>();
        float timer = 0;
        yield return null;
        while (timer < (cooldown - blinkTime))
        {
            if (flipped)
            {
                sprite.color = startColor;
            }
            else
            {
                sprite.color = blinkColor;
            }

            flipped = !flipped;
            timer += blinkTime;
            yield return new WaitForSeconds(blinkTime);
        }
        
        yield return new WaitForSeconds(cooldown - timer);
        sprite.color = startColor;
        canTeleport = true;
        if (objectsToTeleport.Count != 0)
        {
            particles.Play();
            GetComponent<AudioSource>().Play();
        }
    }

    private void Link()
    {
        GameObject[] options = GameObject.FindGameObjectsWithTag("WarpPlatform");
        foreach (GameObject g in options)
        {
            WarpPlatformScript war = g.GetComponent<WarpPlatformScript>();
            if (war)
            {
                war.Register(this.gameObject);
            }
        }
    }

    public void Register(GameObject other)
    {
        if (otherPlatform) return;
        if (other == this.gameObject) return;
        otherPlatform = other;
    }

    public void warp()
    {
        foreach (GameObject g in objectsToTeleport)
        {
            Vector3 offset = g.transform.position - transform.parent.position;
            g.transform.position = otherPlatform.transform.parent.position + offset;
        }
        StartCooldown();
        otherPlatform.GetComponent<WarpPlatformScript>().StartCooldown();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            print("Player detected");
            if (timer <= 0)
            {
                print("Starting timer");
                timer = timeToTeleport;
                if (canTeleport)
                {
                    particles.Play();
                    audio.Play();
                }
            }
            print("Player registered");
            objectsToTeleport.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            print("Player deregistering");
            objectsToTeleport.Remove(other.gameObject);
            if (objectsToTeleport.Count == 0)
            {
                print("No players left, timer deregistered");
                timer = -1;
                particles.Stop();
                //audio.Stop();
            }
        }
    }
}
