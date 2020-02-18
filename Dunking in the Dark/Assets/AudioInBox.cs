using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInBox : MonoBehaviour
{
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.loop = true;
        audio.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        audio.Play();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        audio.Stop();
    }
}
