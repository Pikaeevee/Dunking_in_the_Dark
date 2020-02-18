using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startmenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Debug.Log("Successfully Exited");
        Application.Quit();
    }
    public void ReturntoMenu()
    {
        Debug.Log("back to menu");
        SceneManager.LoadScene(0);
    }

}
