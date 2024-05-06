using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenùScript : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("Tana");
    }

    public void QuitGame()
    {
        Debug.Log("Game quitted.");
        Application.Quit();
    }
}
