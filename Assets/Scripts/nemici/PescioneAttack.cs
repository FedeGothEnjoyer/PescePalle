using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PescioneAttack : MonoBehaviour
{
    [SerializeField] float EnemyPushDuration = 0.5f;
    [SerializeField] float PlayerPushDuration = 0.6f;
    [SerializeField] float attackMagnitude = 5f;
    [SerializeField] float pushbackPlayerMultiplier = 2f;

    public EnemyManager enemyManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!PlayerMovement.dashing && collision.gameObject.layer == 6 && !PlayerMovement.isInflated)
        {
            enemyManager.StartPushBack(enemyManager.targetPositionEnemy, EnemyPushDuration, PlayerPushDuration, attackMagnitude, pushbackPlayerMultiplier, true);
        }
    }

}
