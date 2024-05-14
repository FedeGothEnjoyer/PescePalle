using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialougeHintScript : MonoBehaviour
{
    MessageManager messageManager;
    static DialougeHintScript instance;

    void Start()
    {
        if (instance != null) Destroy(gameObject);
		else
		{
            instance = this;
            DontDestroyOnLoad(this);
            messageManager = MessageManager.instance;
            StartCoroutine(Active());
            SceneManager.activeSceneChanged += Deactivate;
        }
    }

    IEnumerator Active()
    {
        yield return new WaitForSeconds(1);
        messageManager.StartMessage("Clicca sulla tartaruga per parlare.", 100);
		while (!InputSystem.dialogueEnabled)
		{
            yield return null;
		}
        messageManager.StartCoroutine("HideMessage");
    }

    private void Deactivate(Scene s1, Scene s2)
	{
        StopAllCoroutines();
        messageManager.StartCoroutine("HideMessage");
    }
}
