using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string target;
    [SerializeField] Vector2 spawnPosition;
    [SerializeField] bool flipped;
    [SerializeField] bool versoTana;
    [SerializeField] string newDayTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            FindObjectOfType<PlayerMovement>().ChangingRoom(spawnPosition, flipped, target, versoTana, newDayTarget);
        }
    }
}
