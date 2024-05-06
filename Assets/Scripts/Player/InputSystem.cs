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

    static public DialougeManager dialogue;

    // Start is called before the first frame update
    public void Start()
    {
        playerInputEnabled = true;
        enemyMovementEnabled = true;
        dialogueEnabled = false;
        worldEnabled = true;
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

            UnityEngine.UI.Button[] buttons = FindObjectsOfType<UnityEngine.UI.Button>();
            // Check if any UI button contains the point
            foreach (UnityEngine.UI.Button button in buttons)
            {
                // Get the RectTransform of the button
                RectTransform rectTransform = button.GetComponent<RectTransform>();

                // Check if the RectTransform contains the point
                if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main))
                {
                    hitUI = true;
                }
            }

            if (!hitUI && playerInputEnabled)
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
                dialogue.DisplayNextSentence();
			}
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			if (playerInputEnabled)
			{
                player.Dash(mousePos);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerInputEnabled)
            {
                player.Inflate();
            }
        }
    }
}
