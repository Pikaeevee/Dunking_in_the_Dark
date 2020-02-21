using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPlayers : MonoBehaviour
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject goal;
    private Material mat;
    public float p1Distance;
    public float p2Distance;
    public float goalDistance;
    private float[] array;
    public float powerupDistanceDefault;
    public float[] powerupDistances;
    private GameObject[] powerups;
    

    [HideInInspector] public float totalLerp = 1;
    
    //Slow lookup at the start, so we get fast lookups later
    private static readonly int PosOne = Shader.PropertyToID("_PosOne");
    private static readonly int PosTwo = Shader.PropertyToID("_PosTwo");
    private static readonly int OneRad = Shader.PropertyToID("_OneRad");
    private static readonly int TwoRad = Shader.PropertyToID("_TwoRad");
    private static readonly int PosGoal = Shader.PropertyToID("_PosGoal");
    private static readonly int GoalRad = Shader.PropertyToID("_GoalRad");
    private static readonly int Array = Shader.PropertyToID("_Array");
    private static readonly int ArrayLength = Shader.PropertyToID("_ArrayLength");
    private static readonly int Dists = Shader.PropertyToID("_Dists");

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        playerOne = GameObject.FindGameObjectWithTag("Player1");
        playerTwo = GameObject.FindGameObjectWithTag("Player2");
        goal = GameObject.FindGameObjectWithTag("Goal");
        

        powerups = GameObject.FindGameObjectsWithTag("Powerup");
        if (powerups.Length > 10)
        {
            Debug.LogError("The darkness shader cannot handle >10 objects! This is easily fixable, though, so yell at Woody and make him fix it.");
        }
        
        powerupDistances = new float[powerups.Length];
        for (int i = 0; i < powerups.Length; i++)
        {
            powerupDistances[i] = powerupDistanceDefault;
        }
        
        array = new float[powerups.Length * 2];
        for (int i = 0; i < powerups.Length; i++)
        {
            array[2 * i] = powerups[i].transform.position.x;
            array[(2 * i) + 1] = powerups[i].transform.position.y;
        }
    }

    public int Register(GameObject g)
    {
        for (int i = 0; i < powerups.Length; i++)
        {
            if (g == powerups[i])
            {
                return i;
            }
        }
        Debug.LogError("Powerup not found!");
        return -1;
    }

    public void setPowerupSize(int index, float size)
    {
        powerupDistances[index] = size;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        float[] actualDist = new float[powerupDistances.Length];
        for (int i = 0; i < powerupDistances.Length; i++)
        {
            actualDist[i] = powerupDistances[i] * totalLerp;
        }
        
        Vector4 posOne = playerOne.transform.position;
        Vector4 posTwo = playerTwo.transform.position;
        Vector4 posGoal = goal.transform.position;
        mat.SetVector(PosOne, posOne);
        mat.SetVector(PosTwo, posTwo);
        mat.SetFloat(OneRad, p1Distance * totalLerp);
        mat.SetFloat(TwoRad, p2Distance * totalLerp);
        mat.SetVector(PosGoal, posGoal);
        mat.SetFloat(GoalRad, goalDistance * totalLerp);
        
        mat.SetFloatArray(Dists, actualDist);
        mat.SetInt(ArrayLength, array.Length);
        mat.SetFloatArray(Array, array);
    }
}
