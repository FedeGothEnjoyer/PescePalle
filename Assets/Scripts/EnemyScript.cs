using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float speed;
    [SerializeField] float chaseDistance;
    [Header("Debug")]
    [SerializeField] float time_changePosition;
    private float current_time;
    private Vector2 startPos;

    private void OnEnable()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);

        if (distanceToPlayer <= chaseDistance)
        {
            ChasePlayer();
        }
        else
        {
            if (current_time < time_changePosition)
            {
                current_time += Time.deltaTime;
            }
            else
            {
                MoveRandom();
                current_time = 0.0f;
            }
        }
    }
    void ChasePlayer()
    {
        StopAllCoroutines();
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }
    void MoveRandom()
    {
        Vector2 randomDirection = (Random.insideUnitCircle * 1) + startPos;
        StopAllCoroutines();
        StartCoroutine(MoveToPoint(randomDirection));
    }

    IEnumerator MoveToPoint(Vector2 targetPosition)
    {
        while ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
}
