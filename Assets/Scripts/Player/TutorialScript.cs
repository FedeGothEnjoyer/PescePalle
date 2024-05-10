using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    MessageManager messageManager;
    [SerializeField] Collider2D point;

    // Start is called before the first frame update
    void Start()
    {
        messageManager = MessageManager.instance;
        StartCoroutine(StartTutorial());
    }

    IEnumerator StartTutorial()
    {
        InputSystem.playerInputEnabled = false;
        yield return new WaitForSeconds(2);
        messageManager.ActiveMessage("Benvenuto nel tutorial!");
        yield return new WaitForSeconds(5);
        InputSystem.playerInputEnabled = true;
        messageManager.ActiveMessage("Clicca sullo schermo col tasto sinistro del muose per muoverti verso un punto.");

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);
        messageManager.ActiveMessage("Ottimo! Ora segui il percorso e fermati sul punto rosso.");

        while()
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
