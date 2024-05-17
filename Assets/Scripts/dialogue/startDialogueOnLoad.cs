using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startDialogueOnLoad : MonoBehaviour
{
    [SerializeField] string dialogue;
    void Start() 
    {
        StartCoroutine(Show());
    }

    IEnumerator Show()
	{
        yield return null;
        MessageManager.instance.ActiveMessage(dialogue);
        yield return new WaitForSeconds(5.0f);
        MessageManager.instance.DeActiveMessage();
        yield return null;
    }
}
