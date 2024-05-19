using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class FinalDialouge : MonoBehaviour
{
    [SerializeField] Dialouge dialouge;
    [SerializeField] Sprite newDialougeBox;
    [SerializeField] AnimatorOverrideController chase;
    [SerializeField] float speedIncrement = 0.2f;

    DialougeManager dialougeManager;
    Animator animator;

    private bool musicReady = false;
    void Start()
    {
        StartCoroutine(Execute());
        GameObject DialougeBox = GameObject.Find("DialougeBox");
        Image image = DialougeBox.GetComponent<Image>();
        image.sprite = newDialougeBox;
        GameObject name = GameObject.Find("Name");
        Text nameText = name.GetComponent<Text>();
        nameText.color = new Color(1, 0, 0.2588235f);
        GameObject dialouge = GameObject.Find("Dialouge");
        Text dialougeText = dialouge.GetComponent<Text>();
        dialougeText.color = new Color(0.8784314f, 0.03529412f, 0.2745098f);
        dialougeManager = DialougeManager.instance;
    }

    private void Update()
    {
        if(dialougeManager.sentences.Count == 0 && musicReady)
        {
            musicReady = false;
            StartCoroutine(LowerMusic());
        }
    }

    IEnumerator Execute()
    {
        yield return null;

        dialougeManager = DialougeManager.instance;
        animator = transform.GetComponent<Animator>();

        InputSystem.playerInputEnabled = false;

        yield return new WaitForSeconds(2);

        animator.SetBool("startAnim", true);

        yield return new WaitForSeconds(2);

        animator.SetBool("startAnim", false);

        InputSystem.dialogueEnabled = true;
        dialougeManager.StartDialouge(dialouge);
        musicReady = true;
        InputSystem.enemyMovementEnabled = false;
        InputSystem.playerInputEnabled = false;
        dialougeManager.DisplayNextSentence();

        yield return null;
    }

    IEnumerator LowerMusic()
    {
        yield return null;

        for (float i = GameObject.Find("Music").GetComponent<AudioSource>().volume; i >= 0; i *= 0.95f) 
        {
            GameObject.Find("Music").GetComponent<AudioSource>().volume = i;
            yield return new WaitForSeconds(0.05f);
        }
        GameObject.Find("Music").GetComponent<AudioSource>().volume = 0;

        yield return null;
    }

    IEnumerator Chasing()
    {
        yield return null;

        animator.runtimeAnimatorController = chase;

        float speed = 0;
        while (true)
        {
            speed += speedIncrement * Time.deltaTime;
            Vector2 a = PlayerMovement.active.transform.position - transform.position;
            a = a.normalized * speed;
            transform.position = (Vector2)transform.position + a * Time.deltaTime;

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponentInChildren<Collider2D>().enabled = true;
        StartCoroutine(Chasing());
    }
}
