using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

        //Lights and Timing
    [Header("Lights and Timing")]
    [SerializeField] private float gameTime;

    [HideInInspector] public float p1Score;
    [HideInInspector] public float p2Score;

    //Dangerous variable, the delta of our timer
    private float delta = .1f;

    [SerializeField] private float lightsOnTime;
    [SerializeField] private float lightsOffTime;
    [SerializeField] private float lastXLight;
    [SerializeField] private float lightLerpTime;
    [SerializeField] private float playerCircleChange;
    private bool lightsOn = false;
    private float lightCounter;

    [Header("Respawning of players")]
    [SerializeField] private bool respawnRandomly;
    [SerializeField] private Vector2 respawnBoxDimensions;
    [SerializeField] private Vector2[] respawnPoints;

    [Header("Respawning of goals")] 
    [SerializeField] private float goalRespawnTime;
    [SerializeField] private Vector2[] goalPoints;

    //Our Serialized Objects
    [Header("Serialized Inputs")]
    [SerializeField] private GameObject darknessPlane;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI p1Text;
    [SerializeField] private TextMeshProUGUI p2Text;
    private MatchPlayers darknessMatcher;

    // Between round text 
    [SerializeField] private TextMeshProUGUI roundsText; 
    
    //Our players and other objects
    private GameObject p1;
    private GameObject p2;
    private GameObject goal;
    
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
        goal = GameObject.FindGameObjectWithTag("Goal");
        p1Score = 0;//PlayerPrefs.GetFloat("p1Score");
        p2Score = 0;//PlayerPrefs.GetFloat("p2Score");
        
        p1Text.SetText("PLAYER 1\n" + p1Score);
        p2Text.SetText("PLAYER 2\n" + p2Score);

        roundsText.enabled = false; 
        
        //Throw the goal far away, so it look correct
        //goal.transform.position = new Vector3(30,30, goal.transform.position.z);
        
        darknessPlane.SetActive(true);
        darknessMatcher = darknessPlane.GetComponent<MatchPlayers>();
        lightCounter = lightsOnTime;
        StartCoroutine("GameTimeController");
        //updateScore();
        respawnPlayers();
        IEnumerator goalCoroutine = startGoal(goalRespawnTime/2);
        StartCoroutine(goalCoroutine);
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
        scoreText.SetText("TIME LEFT\n" + (int) gameTime);
    }

    IEnumerator interpolateLightsOut(float timer)
    {
        print("Pushing lights out");
        //Wait a frame
        yield return null;
        float startDistance1 = darknessMatcher.p1Distance;
        float endDistance1 = darknessMatcher.p1Distance * playerCircleChange;
        float startDistance2 = darknessMatcher.p2Distance;
        float endDistance2 = darknessMatcher.p2Distance * playerCircleChange;
        float actualTime = 0;
        while (actualTime < timer)
        {
            //Increment time, and normalize if necessary
            actualTime += Time.deltaTime;
            if (actualTime > timer)
            {
                actualTime = timer;
            }

            //Set our values
            
            darknessMatcher.p1Distance = Mathf.Lerp(startDistance1, endDistance1, actualTime / timer);
            darknessMatcher.p2Distance = Mathf.Lerp(startDistance2, endDistance2, actualTime / timer);
            
            //Step 1 frame
            yield return null;
        }
    }
    
    IEnumerator interpolateLightsIn(float timer)
    {
        
        //Wait a frame
        yield return null;
        float startDistance1 = darknessMatcher.p1Distance;
        float endDistance1 = darknessMatcher.p1Distance / playerCircleChange;
        float startDistance2 = darknessMatcher.p2Distance;
        float endDistance2 = darknessMatcher.p2Distance / playerCircleChange;
        float actualTime = 0;
        while (actualTime < timer)
        {
            actualTime += Time.deltaTime;
            if (actualTime > timer)
            {
                actualTime = timer;
            }
            darknessMatcher.p1Distance = Mathf.Lerp(startDistance1, endDistance1, actualTime / timer);
            darknessMatcher.p2Distance = Mathf.Lerp(startDistance2, endDistance2, actualTime / timer);
            yield return null;
        }
    }
    
    private void SetLights()
    {
        print("Starting set lights functions");
        IEnumerator co;
        if (lightsOn)
        {
            //Turn the lights on!
            co = interpolateLightsOut(lightLerpTime);
        }
        else
        {
            co = interpolateLightsIn(lightLerpTime);
        }

        StartCoroutine(co);
    }

    IEnumerator respawnGoal(float time)
         {
             //Halt a frame, for correct timing
             yield return null;
             
             float currentTime = 0;
             while (currentTime < time / 2)
             {
                 currentTime += Time.deltaTime;
                 darknessMatcher.totalLerp = Mathf.Lerp(1, 0, currentTime * 2 / time);
                 yield return null;
             }
             if (goalPoints.Length == 0)
             {
                 Debug.LogError("There are not goal respawning points! Must be >0.");
             }
     
             Vector2 point = goalPoints[Random.Range(0, goalPoints.Length)];
             goal.transform.position = new Vector3(point.x, point.y, goal.transform.position.z);
             
             //Reset darkness
             while (currentTime < time)
             {
                 currentTime += Time.deltaTime;
                 darknessMatcher.totalLerp = Mathf.Lerp(0, 1, (currentTime - (time/2)) * 2f / time);
                 yield return null;
             }
         }
    
    IEnumerator startGoal(float time)
    {
        //Halt a frame, for correct timing
        yield return null;
        float currentTime = 0;
        if (goalPoints.Length == 0)
        {
            Debug.LogError("There are not goal respawning points! Must be >0.");
        }

        Vector2 point = goalPoints[0];
        goal.transform.position = new Vector3(point.x, point.y, goal.transform.position.z);
        
        //Reset darkness
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            darknessMatcher.totalLerp = Mathf.Lerp(0, 1, (currentTime / time));
            yield return null;
        }
        print("Finished!");
    }

    private void ExitGame()
    {
        //Best of 3
        print("Game should be exiting!");
        int winner;
        if (p1Score > p2Score)
        {
            float p1Final = PlayerPrefs.GetFloat("p1Score");
            p1Final += 1;
            PlayerPrefs.SetFloat("p1Score", p1Final);
            if (p1Final >= 1.9f)
            {
                //End game, p1 wins!
                PlayerPrefs.SetInt("winner", 1);
                SceneManager.LoadScene("Win Screen");
            }
            else
            {
                StartCoroutine(RoundEnd(1, PlayerPrefs.GetFloat("p1Score"), PlayerPrefs.GetFloat("p2Score")));
            }
            
        }
        else if (p2Score > p1Score)
        {
            float p2Final = PlayerPrefs.GetFloat("p2Score");
            p2Final += 1;
            PlayerPrefs.SetFloat("p2Score", p2Final);
            if (p2Final >= 1.9f)
            {
                //End game, p1 wins!
                PlayerPrefs.SetInt("winner", 2);
                SceneManager.LoadScene("Win Screen");
            }
            else
            {
                StartCoroutine(RoundEnd(2, PlayerPrefs.GetFloat("p1Score"), PlayerPrefs.GetFloat("p2Score"))); 
            }
        }
        else
        {
            print("Tie! No points!");
            StartCoroutine(RoundEnd(0, PlayerPrefs.GetFloat("p1Score"), PlayerPrefs.GetFloat("p2Score")));
        }
        
        
        
        
        /*
        //Normal
        float gameCount = PlayerPrefs.GetFloat("gameCount");
        gameCount++;
        if (gameCount >= (PlayerPrefs.GetFloat("maxGames") - .1f))
        {
            //End the game!
            print("Game should be exiting!");
            int winner;
            if (p1Score > p2Score)
            {
                winner = 1;
            }
            else
            {
                winner = 2;
            }
            Debug.Log("this is winner: " + winner);
            PlayerPrefs.SetInt("winner", winner);
            Debug.Log("this is set in PlayerPrefs: " + PlayerPrefs.GetInt("winner"));
            SceneManager.LoadScene("Win Screen");
        }
        else
        {
            PlayerPrefs.SetFloat("gameCount", gameCount);
            PlayerPrefs.SetFloat("p1Score", p1Score);
            PlayerPrefs.SetFloat("p2Score", p2Score);
            SceneManager.LoadScene(Random.Range(1, 3));
        }*/
        
    }

    private int getNextMap()
    {
        int currMap = SceneManager.GetActiveScene().buildIndex;
        int nextMap = Random.Range(1, 3);
        while (nextMap == currMap)
        {
            nextMap = Random.Range(1, 3);
        }
        return nextMap;
    }

    
    

    public void addP1()
    {
        p1Score++;
        StartCoroutine(updateScore(goalRespawnTime/2));
    }

    public void addP2()
    {
        p2Score++;
        StartCoroutine(updateScore(goalRespawnTime/2));
    }

    IEnumerator updateScore(float delay)
    {
        p1Text.SetText("PLAYER 1\n" + p1Score);
        p2Text.SetText("PLAYER 2\n" + p2Score);
        
        StartCoroutine(respawnGoal(goalRespawnTime));
        yield return new WaitForSeconds(delay);
        
        respawnPlayers();
    }

    private void respawnPlayers()
    {
        print("Respawning Players!");
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
        print("Players Respawned");
    }

    private void OnDrawGizmos()
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
        
        //Set up goal points
        Gizmos.color = Color.red;
        foreach (Vector2 v in goalPoints)
        {
            Gizmos.DrawWireSphere(new Vector3(v.x, v.y,0), .2f);
        }
    }

    // display who won the round, move to next map 
    private IEnumerator RoundEnd(int winner, float p1score, float p2score)
    {
        int p1s = (int)p1score;
        int p2s = (int)p2score;
        // display the match winner, move to next round 
        if (winner == 1)
        {
            roundsText.SetText("P1 wins the round! \n" + p1s + " - " + p2s); 
        }
        else if (winner == 2)
        {
            roundsText.SetText("P2 wins the round! \n" + p1s + " - " + p2s);
        }
        // tie 
        else
        {
            roundsText.SetText("Round tied! \n" + p1s + " - " + p2s);
        }
        roundsText.enabled = true; 

        // load next scene after delay 
        yield return new WaitForSeconds(1.5f);

        int nextMap = getNextMap();
        SceneManager.LoadScene(nextMap);
    }
}
