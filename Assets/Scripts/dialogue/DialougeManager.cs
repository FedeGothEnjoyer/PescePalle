using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialougeManager : MonoBehaviour
{
    public Text nameText;
    public Text dialougeText;

    public Animator animator;
    public UnityEvent dialougeEnded;

    [HideInInspector] public Queue<string> sentences;

    static public DialougeManager instance;

    private void Awake()
    {
        instance = this;
        dialougeEnded = new UnityEvent();
    }

    void Start()
    {


        if (instance != null && instance != this)
		{
            Destroy(gameObject);
            return;
		}
        instance = this;
        sentences = new Queue<string>();
    }

	public void StartDialouge(Dialouge dialouge)
    {
        if (PlayerMovement.isAttacked || PlayerMovement.transitionStage != 0)
            return;

        if (nameText == null) nameText = GameObject.Find("Name").GetComponent<Text>();

        InputSystem.playerInputEnabled = false;
        //Avvio l'animazione
        animator = GameObject.Find("DialougeBox").GetComponent<Animator>();
        animator.SetBool("IsOpen", true);
        nameText.text = dialouge.name;

        sentences.Clear();
        foreach (string sentence in dialouge.sentences)
        {
            sentences.Enqueue(sentence);
        }
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialouge();
            return;
        }
        
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialougeText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialougeText.text += letter;
            yield return null;
        }
    }

    void EndDialouge()
    {
        Debug.Log("end dialouge");
        dialougeEnded.Invoke();
        //Sblocca l'input
        InputSystem.dialogueEnabled = false;
        InputSystem.playerInputEnabled = true;
        InputSystem.enemyMovementEnabled = true;
        //Animazione di chiusura
        animator.SetBool("IsOpen", false);
    }


}
