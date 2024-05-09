using Pathfinding.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    [SerializeField] float fadeSpeed = 5f;
    public Text messageText;
    static public MessageManager instance;

    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartMessage(string message, float duration)
    {
        if (message == null)
        {
            Debug.LogError("Message is null.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(PopupMessage(message, duration));
    }

    IEnumerator PopupMessage(string message, float duration)
    {
        messageText.text = message;

        messageText.enabled = true;

        Color newColor = messageText.color;

        newColor.a = 1;
        while (messageText.color.a < 0.95f)
        {
            messageText.color = Color.Lerp(messageText.color, newColor, fadeSpeed * Time.deltaTime);
            yield return null;
            Debug.Log(messageText.enabled);
        }

        yield return new WaitForSeconds(duration);

        newColor.a = 0;
        while (messageText.color.a > 0)
        {
            messageText.color = Color.Lerp(messageText.color, newColor, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        messageText.enabled = false;
        Debug.Log(messageText.enabled);
    }
}
