using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputSystem : MonoBehaviour
{
    static public PlayerMovement player;
    static public bool playerInputEnabled;
    static public bool dialogueEnabled;
    static public bool enemyMovementEnabled;
    static public bool worldEnabled;
    static public bool selectedInteractableEnabled;
    static public bool playerMovementEnabled; //tutorial
    static public bool dashEnabled;
    static public bool inflateEnabled;

    static public DialougeManager dialogue;

    // Start is called before the first frame update
    public void Start()
    {
        playerInputEnabled = true;

        playerMovementEnabled = true;
        enemyMovementEnabled = true;
        dialogueEnabled = false;
        worldEnabled = true;
        selectedInteractableEnabled = true;
        dashEnabled = true;
        inflateEnabled = true;
        player = GetComponent<PlayerMovement>();
        dialogue = GetComponentInChildren<DialougeManager>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
		{
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            bool hitUI = false;

            Button[] buttons = FindObjectsOfType<UnityEngine.UI.Button>();
            // Check if any UI button contains the point
            foreach (Button button in buttons)
            {
                // Get the RectTransform of the button
                RectTransform rectTransform = button.GetComponent<RectTransform>();

                // Check if the RectTransform contains the point
                if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main))
                {
                    hitUI = true;
                }
            }

            if (!hitUI && playerInputEnabled && playerMovementEnabled)
			{
                bool objectClicked = false;
                foreach (InteractableBehaviour b in FindObjectsByType<InteractableBehaviour>(FindObjectsSortMode.None))
                {
                    if (objectClicked = b.Clicked(mousePos))
                    {
                        dialogueEnabled = true;
                        enemyMovementEnabled = false;
                        break;
                    }
                }
                if (objectClicked) player.MoveToObject(mousePos);
                else player.MoveToPosition(mousePos);
            }

            if (dialogueEnabled)
			{
                if (dialogue == null) dialogue = GameObject.Find("DialougeManager").GetComponent<DialougeManager>();
                dialogue.DisplayNextSentence();
			}
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			if (playerInputEnabled && dashEnabled)
			{
                player.Dash(mousePos);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerInputEnabled && inflateEnabled)
            {
                player.Inflate();
            }
        }
    }
}
