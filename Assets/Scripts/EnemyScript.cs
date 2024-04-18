using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float speed;
    [SerializeField] float chaseDistance;
    [SerializeField] float wallStopRange;
    [SerializeField] float scoutingRange;
    [SerializeField] float turnSpeed;
    public static int isChasing; 
    [Header("Debug")]
    [SerializeField] float time_changePosition;
    private float current_time;
    private Vector2 startPos;
    private bool canChase = true;

    //Per evitare che 'isChasing' incrementi o decrementi ad ogni frame
    private bool chased = false;

    private void OnEnable()
    {
        startPos = transform.position;
        isChasing = 0;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);

        if (InputSystem.enemyMovementEnabled)
        {
            if (distanceToPlayer <= chaseDistance && canChase)
            {
                if (!chased)
                {
                    chased = true;
                    isChasing += 1;
                }
                ChasePlayer();
            }
            else
            {
                if(PlayerMovement.isInflated == false)
                {
                    canChase = true;
                }
                if(chased)
                {
                    chased = false;
                    isChasing -= 1;
                }
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
    }
    void ChasePlayer()
    {
        StopAllCoroutines();
        Vector2 targetPos = target.transform.position;
        CheckForWall(ref targetPos, true);
        transform.position = Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime);

    }
    void MoveRandom()
    {
        Vector2 randomDirection = (Random.insideUnitCircle * scoutingRange) + startPos;
        StopAllCoroutines();
        CheckForWall(ref randomDirection, false);
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

    void CheckForWall(ref Vector2 targetPosition, bool chasing)
    {
        var collider = GetComponent<CircleCollider2D>();
        RaycastHit2D ray = Physics2D.CircleCast(transform.position,collider.radius, -((Vector2)transform.position - targetPosition).normalized, 10f, LayerMask.GetMask("walls"));
        if (ray.collider != null && ray.distance < ((Vector2)transform.position - targetPosition).magnitude && (targetPosition - ray.point).sqrMagnitude < (targetPosition - (Vector2)transform.position).sqrMagnitude)
        {
            var vec = ray.point - (Vector2)transform.position;
            var vec2 = Vector2.ClampMagnitude(vec, wallStopRange);
            targetPosition = (Vector2)transform.position + vec - vec2;

            //if(chasing) startPos = ray.point;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6 && PlayerMovement.isInflated)
        {
            MoveRandom();
            canChase = false;
        }
    }
}
