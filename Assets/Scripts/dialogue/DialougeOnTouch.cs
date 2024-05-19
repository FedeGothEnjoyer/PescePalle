using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DialougeOnTouch : MonoBehaviour
{
    [SerializeField] Dialouge dialouge;
    DialougeManager dialougeManager;
    bool first;
    private void Start()
    {
        first = true;
        dialougeManager = DialougeManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Execute());
    }

    IEnumerator Execute()
    {
        yield return null;

        if (first)
        {
            InputSystem.dialogueEnabled = true;
            dialougeManager.StartDialouge(dialouge);
            InputSystem.enemyMovementEnabled = false;
            InputSystem.playerInputEnabled = false;
            first = false;
            dialougeManager.DisplayNextSentence();
            InputSystem.clickEnabled = false;
        }

        yield return new WaitForSeconds(2);
        InputSystem.clickEnabled = true;

        yield return null;
    }
}
