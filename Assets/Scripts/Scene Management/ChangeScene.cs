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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            EnemyManager.chasingCount = 0;
            PlayerMovement.isInflated = false;
            FindObjectOfType<PlayerMovement>().ChangingRoom(spawnPosition, flipped, target, versoTana);
        }
    }
}