using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPlayers : MonoBehaviour
{
    [SerializeField] private GameObject playerOne;
    [SerializeField] private GameObject playerTwo;
    [SerializeField] private Material mat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector4 posOne = playerOne.transform.position;
        Vector4 posTwo = playerTwo.transform.position;
        mat.SetVector("_PosOne", posOne);
        mat.SetVector("_PosTwo", posTwo);
    }
}
