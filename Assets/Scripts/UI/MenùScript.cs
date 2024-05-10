using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenùScript : MonoBehaviour
{
    [SerializeField] List<SceneAsset> taneGiorni;

    public void StartNewGame()
    {
        SceneManager.LoadScene("Tana");
        CurrentData.day = 1;
    }

    public void ContinueGame()
    {
        DataSystem.LoadData();
        SceneManager.LoadScene(taneGiorni[CurrentData.day-1].name);
    }

    public void QuitGame()
    {
        Debug.Log("Game quitted.");
        Application.Quit();
    }
}
