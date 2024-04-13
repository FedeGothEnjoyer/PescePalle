using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialougeManager : MonoBehaviour
{
    //Testi
    public Text nameText;
    public Text dialougeText;
    //Animazione
    public Animator animator;
    //Frasi
    private Queue<string> senteces;

    void Start()
    {//Init della queue
        senteces = new Queue<string>();
    }

    public void StartDialouge(Dialouge dialouge)
    {
        //FIX : TODO : fare in modo che ci sia un delay di chiamata della funzione dopo il click
        //all'interagibile in modo tale che prima venga spostato il pesce e poi venga fatto vedere il popup del
        //messaggio

        //Blocco Input
        InputSystem.playerInputEnabled = false;
        //Avvio l'animazione
        animator.SetBool("IsOpen", true);
        //Metto il nome del personaggio che parla nel popup
        nameText.text = dialouge.name;

        senteces.Clear();
        //Salvo le frasi
        foreach (string sentence in dialouge.sentences)
        {
            senteces.Enqueue(sentence);
        }
    }
    public void DisplayNextSentence()
    {
        if (senteces.Count == 0)
        {//Fine se finisco le frasi
            EndDialouge();
            return;
        }
        //Prendo la frase e avvio una co-routine, per fare l'animazione di stampa
        string sentence = senteces.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        //Animazione di stampa
        //Co-routine perchè così posso interrompere facilmente l'anim.
        dialougeText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialougeText.text += letter;
            //yield = aspetto un frame
            yield return null;
        }
    }

    void EndDialouge()
    {
        //Sblocca l'input
        InputSystem.dialogueEnabled = false;
        InputSystem.playerInputEnabled = true;
        //Animazione di chiusura
        animator.SetBool("IsOpen", false);
    }


}
