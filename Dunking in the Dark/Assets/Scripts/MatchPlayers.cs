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
    public float powerupDistance = 1;

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
    private static readonly int PowRad = Shader.PropertyToID("_PowRad");

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        playerOne = GameObject.FindGameObjectWithTag("Player1");
        playerTwo = GameObject.FindGameObjectWithTag("Player2");
        goal = GameObject.FindGameObjectWithTag("Goal");
        

        GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
        array = new float[powerups.Length * 2];
        for (int i = 0; i < powerups.Length; i++)
        {
            array[2 * i] = powerups[i].transform.position.x;
            array[(2 * i) + 1] = powerups[i].transform.position.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector4 posOne = playerOne.transform.position;
        Vector4 posTwo = playerTwo.transform.position;
        Vector4 posGoal = goal.transform.position;
        mat.SetVector(PosOne, posOne);
        mat.SetVector(PosTwo, posTwo);
        mat.SetFloat(OneRad, p1Distance * totalLerp);
        mat.SetFloat(TwoRad, p2Distance * totalLerp);
        mat.SetVector(PosGoal, posGoal);
        mat.SetFloat(GoalRad, goalDistance * totalLerp);
        
        mat.SetFloat(PowRad, powerupDistance);
        mat.SetInt(ArrayLength, array.Length);
        mat.SetFloatArray(Array, array);
    }
}
