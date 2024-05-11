using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] SceneAsset target;
    [SerializeField] Vector2 spawnPosition;
    [SerializeField] bool flipped;
    [SerializeField] bool versoTana;
    [SerializeField] SceneAsset newDayTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            FindObjectOfType<PlayerMovement>().ChangingRoom(spawnPosition, flipped, target, versoTana, newDayTarget);
        }
    }
}
