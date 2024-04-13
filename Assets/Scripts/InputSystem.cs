using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    static public PlayerMovement player;
    static public bool playerInputEnabled;
    static public bool dialogueEnabled;

    DialougeManager dialogue;

    // Start is called before the first frame update
    void Start()
    {
        playerInputEnabled = true;
        dialogueEnabled = false;
        player = GetComponent<PlayerMovement>();
        dialogue = FindAnyObjectByType<DialougeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
		{
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (playerInputEnabled)
			{
                bool objectClicked = false;
                foreach (InteractableBehaviour b in FindObjectsByType<InteractableBehaviour>(FindObjectsSortMode.None))
                {
                    if (objectClicked = b.Clicked(mousePos))
                    {
                        dialogueEnabled = true;
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

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}
