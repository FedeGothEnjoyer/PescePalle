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

    Color newColor;

    private void Start()
    {
        newColor = messageText.color;
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
    public void StartMessage(string message, float duration)
    {
        if (message == null)
        {
            Debug.LogError("Message is null.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(MessageDuration(message, duration));
    }

    public void ActiveMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessage(message));
    }

    public void DeActiveMessage()
    {
        StopAllCoroutines();
        StartCoroutine(HideMessage());
    }

    IEnumerator MessageDuration(string message, float duration)
    {
        messageText.text = message;
        messageText.enabled = true;

        newColor.a = 1;
        while (messageText.color.a < 0.95f)
        {
            messageText.color = Color.Lerp(messageText.color, newColor, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        newColor.a = 0;
        while (messageText.color.a > 0)
        {
            messageText.color = Color.Lerp(messageText.color, newColor, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        messageText.enabled = false;
    }

    public IEnumerator ShowMessage(string message)
    {
        messageText.text = message;
        messageText.enabled = true;

        newColor.a = 1;
        while (messageText.color.a < 0.95f)
        {
            messageText.color = Color.Lerp(messageText.color, newColor, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator HideMessage()
    {
        newColor.a = 0;
        while (messageText.color.a > 0)
        {
            messageText.color = Color.Lerp(messageText.color, newColor, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        messageText.enabled = false;
    }
}
