using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager Instance;
    public GameManager instance
    {
        get
        {
            if (Instance == null)
            {
                Instance = this;
            }
            return Instance;
        }
        set
        {
            Instance = value;
        }
    }

    //Lights and Timing
    [SerializeField] private float gameTime;

    //Dangerous variable, the delta of our timer
    private float delta = .1f;

    [SerializeField] private float lightsOnTime;
    [SerializeField] private float lightsOffTime;
    [SerializeField] private float lastXLight;
    private bool lightsOn = true;
    private float lightCounter;

    //Our actual light object
    [SerializeField] private GameObject darknessPlane;


    // Start is called before the first frame update
    void Start()
    {
        lightCounter = lightsOnTime;
        StartCoroutine("GameTimeController");
        SetLights();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GameTimeController()
    {
        while (gameTime > lastXLight)
        {
            SetTimer();
            gameTime -= delta;
            lightCounter -= delta;
            print("LightCounter is " + lightCounter);
            if (lightCounter < 0)
            {
                print("Switching lights!");
                lightsOn = !lightsOn;
                lightCounter = lightsOn ? lightsOnTime : lightsOffTime;
                SetLights();
            }
            yield return new WaitForSeconds(delta);
        }
        lightsOn = true;
        SetLights();
        while (gameTime > 0)
        {
            SetTimer();
            gameTime -= delta;
            yield return new WaitForSeconds(delta);
        }
        SetTimer();
        //At the end of our checks! Time is out, so lets exit
        ExitGame();
    }

    private void SetTimer()
    {
        //Stub for setting the time!
        print("Setting time to " + gameTime);
    }

    //TODO: Fill out my stubs!
    private void SetLights()
    {
        if (lightsOn)
        {
            //Turn the lights on!
            darknessPlane.SetActive(false);
        }
        else
        {
            //Turn them off!
            darknessPlane.SetActive(true);
        }
    }

    private void ExitGame()
    {
        print("Game should be exiting!");
    }
}
