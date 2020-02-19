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

    [HideInInspector] public float totalLerp = 1;
    
    //Slow lookup at the start, so we get fast lookups later
    private static readonly int PosOne = Shader.PropertyToID("_PosOne");
    private static readonly int PosTwo = Shader.PropertyToID("_PosTwo");
    private static readonly int OneRad = Shader.PropertyToID("_OneRad");
    private static readonly int TwoRad = Shader.PropertyToID("_TwoRad");
    private static readonly int PosGoal = Shader.PropertyToID("_PosGoal");
    private static readonly int GoalRad = Shader.PropertyToID("_GoalRad");

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        playerOne = GameObject.FindGameObjectWithTag("Player1");
        playerTwo = GameObject.FindGameObjectWithTag("Player2");
        goal = GameObject.FindGameObjectWithTag("Goal");
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
    }
}
