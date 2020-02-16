using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class victor : MonoBehaviour
{
    public TextMeshProUGUI win;
// Start is called before the first frame update
void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("winner"));
        win.SetText("PLAYER " + PlayerPrefs.GetInt("winner") +  " WINS");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
