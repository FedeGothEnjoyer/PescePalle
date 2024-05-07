using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public void SaveAndQuit()
    {
        InputSystem.playerInputEnabled = true;
        InputSystem.selectedInteractableEnabled = true;
        Destroy(PlayerMovement.active.gameObject);
        Time.timeScale = 1;
        DataSystem.SaveData();
        SceneManager.LoadScene("Menù");
    }

    public void PauseGame()
    {
        InputSystem.playerInputEnabled = false;
        InputSystem.selectedInteractableEnabled = false;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        InputSystem.playerInputEnabled = true;
        InputSystem.selectedInteractableEnabled = true;
        Time.timeScale = 1;
    }
}
