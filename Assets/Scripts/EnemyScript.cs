using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;

public class EnemyScript : MonoBehaviour
{
    private SpriteRenderer SR;
    private Animator animator;
    [SerializeField] Sprite pushedEnemy;
    [SerializeField] GameObject target;
    [SerializeField] float speed = 2;
    [SerializeField] float chaseDistance = 3;
    [SerializeField] float wallStopRange = 2;
    [SerializeField] float scoutingRange = 3;
    //[SerializeField] float turnSpeed = 2;
    [SerializeField] float time_changePosition = 0.2f;
    [SerializeField] float pushMagnitude = 3.0f;
    [SerializeField] float pushMagnitudeReducerPufferFish = 0.35f;
    [SerializeField] float pushDuration = 1f;
    public static int isChasing;
    private float current_time;
    private Vector2 startPos;
    private Vector2 targetPos;
    private bool canChase = true;
    private bool canReturn = true;
    private bool isColliding = false;
    private Sprite normalEnemy;

    //Per evitare che 'isChasing' incrementi o decrementi ad ogni frame
    private bool chased = false;

    private void OnEnable()
    {
        SR = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        startPos = transform.position;
        isChasing = 0;
        normalEnemy = SR.sprite;
    }

    void Update()
    {
        SR.flipX = targetPos.x > transform.position.x;
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
                if (PlayerMovement.isInflated == false)
                {
                    canChase = true;
                }
                if (chased)
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
                    if(canReturn) MoveRandom();

                    current_time = 0.0f;
                }
            }
        }
    }
    void ChasePlayer()
    {
        StopAllCoroutines();
        targetPos = target.transform.position;
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

    IEnumerator PushBackEnemy(Vector2 direction, float magnitude, float duration)
    {
        float elapsedTime = 0.0f;
        Vector2 initialPosition = transform.position;
        Vector2 targetPosition = initialPosition - direction * magnitude;
        canReturn = false;
        animator.enabled = false;
        SR.sprite = pushedEnemy;
        while (elapsedTime < duration)
        {
            float t = 1 - Mathf.Pow(1 - (elapsedTime / duration), 4);

            Vector2 currentPos = Vector2.Lerp(initialPosition, targetPosition, t);
            transform.position = new Vector2(currentPos.x, currentPos.y);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        animator.enabled = true;
        canReturn = true;
        transform.position = targetPosition;
    }
    IEnumerator PushBackPufferFish(Vector2 direction, float magnitude, float duration, Collider2D collision)
    {
        float elapsedTime = 0.0f;
        Vector2 initialPosition = collision.transform.position;
        Vector2 targetPosition = initialPosition - direction * magnitude * pushMagnitudeReducerPufferFish;

        InputSystem.playerInputEnabled = false;
        while (elapsedTime < duration)
        {
            float t = 1 - Mathf.Pow(1 - (elapsedTime / duration), 4);

            Vector2 currentPos = Vector2.Lerp(initialPosition, targetPosition, t);
            collision.transform.position = new Vector2(currentPos.x, currentPos.y);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        InputSystem.playerInputEnabled = true;
    }

    void CheckForWall(ref Vector2 targetPosition, bool chasing)
    {
        var collider = GetComponent<CircleCollider2D>();
        RaycastHit2D ray = Physics2D.CircleCast(transform.position, collider.radius, -((Vector2)transform.position - targetPosition).normalized, 10f, LayerMask.GetMask("walls"));
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
        if (collision.gameObject.layer == 6 && PlayerMovement.isInflated)
            InflatedEffect(ref collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && PlayerMovement.isInflated && !isColliding)
            InflatedEffect(ref collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isColliding = false;
        }
    }

    private void InflatedEffect(ref Collider2D collision)
    {

        isColliding = true;

        Vector2 targetPosition_enemy = (collision.transform.position - transform.position).normalized;
        Vector2 targetPosition_pufferFish = (transform.position - collision.transform.position).normalized;

        StartCoroutine(PushBackEnemy(targetPosition_enemy, pushMagnitude, pushDuration));
        StartCoroutine(PushBackPufferFish(targetPosition_pufferFish, pushMagnitude, pushDuration, collision));

        canChase = false;
    }
}
