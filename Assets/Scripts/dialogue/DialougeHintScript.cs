using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeHintScript : MonoBehaviour
{
    MessageManager messageManager;
    void Start()
    {
        messageManager = MessageManager.instance;
        StartCoroutine(Active());
    }

    IEnumerator Active()
    {
        yield return new WaitForSeconds(1);
        messageManager.StartMessage("Clicca sulla tartaruga per parlare.", 5);
    }
}
