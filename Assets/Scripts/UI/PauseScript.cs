using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public void SaveAndQuit()
    {
        Destroy(PlayerMovement.active.gameObject);
        Time.timeScale = 1;
        DataSystem.SaveData();
        SceneManager.LoadScene("Menù");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
