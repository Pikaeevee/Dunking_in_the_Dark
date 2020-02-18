using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPlatformScript : MonoBehaviour
{
    private GameObject otherPlatform;

    [SerializeField] private float timeToTeleport = 1f;

    private ArrayList objectsToTeleport;
    private float timer;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        print("Timer is " + timer);
        if (timer > 0)
        {
            print("Timer subtracted");
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                print("Warping!");
                timer = -1;
                warp();
            }
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
            }
        }
    }
}
