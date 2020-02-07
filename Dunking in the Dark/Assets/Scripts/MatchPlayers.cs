using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPlayers : MonoBehaviour
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private Material mat;
    [SerializeField] private float p1Distance;
    [SerializeField] private float p2Distance;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().sharedMaterial;
        playerOne = GameObject.FindGameObjectWithTag("Player1");
        playerTwo = GameObject.FindGameObjectWithTag("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        Vector4 posOne = playerOne.transform.position;
        Vector4 posTwo = playerTwo.transform.position;
        mat.SetVector("_PosOne", posOne);
        mat.SetVector("_PosTwo", posTwo);
        mat.SetFloat("_OneRad", p1Distance);
        mat.SetFloat("_TwoRad", p2Distance);
    }
}
