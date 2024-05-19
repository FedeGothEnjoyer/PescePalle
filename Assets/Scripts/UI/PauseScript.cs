using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    private bool lastInput;
    private bool message = false;
    GameObject messageUI;
    private void Start()
    {
        messageUI = GameObject.Find("Message");
    }
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
        if (messageUI.activeSelf && SceneManager.GetActiveScene().name == "Tuto")
        {
            messageUI.SetActive(false);
            message = true;
        }

        lastInput = InputSystem.playerInputEnabled;
        InputSystem.playerInputEnabled = false;
        InputSystem.selectedInteractableEnabled = false;
        InputSystem.paused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if (message)
        {
            messageUI.SetActive(true);
            message = false;
        }

        InputSystem.playerInputEnabled = lastInput;
        InputSystem.selectedInteractableEnabled = true;
        InputSystem.paused = false;
        Time.timeScale = 1;
    }
}
