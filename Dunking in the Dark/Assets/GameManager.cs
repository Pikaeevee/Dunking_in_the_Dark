using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

        //Lights and Timing
    [SerializeField] private float gameTime;

    [HideInInspector] public float p1Score;
    [HideInInspector] public float p2Score;

    //Dangerous variable, the delta of our timer
    private float delta = .1f;

    [SerializeField] private float lightsOnTime;
    [SerializeField] private float lightsOffTime;
    [SerializeField] private float lastXLight;
    private bool lightsOn = true;
    private float lightCounter;

    [SerializeField] private bool respawnRandomly;
    [SerializeField] private Vector2 respawnBoxDimensions;
    [SerializeField] private Vector2[] respawnPoints;

    //Our Serialized Objects
    [SerializeField] private GameObject darknessPlane;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI p1Text;
    [SerializeField] private TextMeshProUGUI p2Text;
    
    //Our players
    private GameObject p1;
    private GameObject p2;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!respawnRandomly && respawnPoints.Length == 0)
        {
            Debug.LogError("GameManager must have points chosen for non-random spawning!");
            Debug.Break();
        }
        //Get other objects
        p1 = GameObject.FindGameObjectWithTag("Player1");
        p2 = GameObject.FindGameObjectWithTag("Player2");
        
        lightCounter = lightsOnTime;
        StartCoroutine("GameTimeController");
        SetLights();
        updateScore();
        instance = this;
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
        scoreText.SetText("TIME LEFT:\n" + (int) gameTime);
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

    public void addP1()
    {
        p1Score++;
        updateScore();
    }

    public void addP2()
    {
        p2Score++;
        updateScore();
    }

    public void updateScore()
    {
        p1Text.SetText("PLAYER 1\n" + p1Score);
        p2Text.SetText("PLAYER 2\n" + p2Score);
        
        //Set the players positions!
        Vector3 newPos = Vector3.zero;
        if (respawnRandomly)
        {
            float x = transform.position.x + Random.Range(-1 * respawnBoxDimensions.x, respawnBoxDimensions.x);
            float y = transform.position.y + Random.Range(-1 * respawnBoxDimensions.y, respawnBoxDimensions.y);
            newPos = new Vector3(x, y, 2);
        }
        else
        {
            newPos = respawnPoints[Random.Range(0, respawnPoints.Length)];
        }

        p1.transform.position = newPos + new Vector3(-1, 0, 0);
        p2.transform.position = newPos + new Vector3(1, 0, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (respawnRandomly)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(respawnBoxDimensions.x, respawnBoxDimensions.y, .1f));
        }
        else
        {
            foreach (Vector3 v in respawnPoints)
            {
                Gizmos.DrawWireSphere(v, .2f);
            }
        }
    }
}
