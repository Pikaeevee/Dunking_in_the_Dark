using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [SerializeField] private float maxGames;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("p1Score", 0);
        PlayerPrefs.SetFloat("p2Score", 0);
        PlayerPrefs.SetFloat("gameCount", 0);
        PlayerPrefs.SetFloat("winner", 0);
        PlayerPrefs.SetFloat("maxGames", maxGames);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
