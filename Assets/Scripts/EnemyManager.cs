using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float scoutingRange = 1f;
    [SerializeField] float changeIdlePositionRate = 0.5f;

    [SerializeField] float chaseDistance = 3f;

    [SerializeField] float pushBackMagnitude = 20f;
    [SerializeField] float pushBackMagnitudeReduceForPlayer = 0.2f;
    [SerializeField] float pushBackDuration = 2f;

    Seeker seeker;
    AIPath aiPath;
    Rigidbody2D rigidBody;

    private bool isColliding = false; //determines if enemy is colliding with the player
    private bool isPushedBack = false; //used to stop searching for idle position
    private bool forceIdle = false; //forces idle when player is inflated
    private float currentTimeChangeIdlePosition = 0f;
    Vector2 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        seeker = transform.parent.GetComponent<Seeker>();
        aiPath = transform.parent.GetComponent<AIPath>();
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = transform.parent.position;
        aiPath.canSearch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystem.enemyMovementEnabled)
        {
            aiPath.canMove = true;
            float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);

            //chase player
            if (!forceIdle && distanceToPlayer <= chaseDistance)
            {
                aiPath.canSearch = true;
            }
            else
            {
                aiPath.canSearch = false;

                //change idle position
                if (!isPushedBack && currentTimeChangeIdlePosition >= changeIdlePositionRate)
                {
                    currentTimeChangeIdlePosition = 0;
                    IdlePosition();
                }
                else
                {
                    currentTimeChangeIdlePosition += Time.deltaTime;
                }

                if (!PlayerMovement.isInflated)
                    forceIdle = false;
            }

        }
        else
        {
            aiPath.canMove = false;
        }
    }

    void IdlePosition()
    {
        Vector2 randomDirection = (Random.insideUnitCircle * scoutingRange) + startPosition;
        seeker.StartPath(transform.parent.position, randomDirection);
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

    void InflatedEffect(ref Collider2D collision)
    {
        isColliding = true;
        Vector2 targetPositionEnemy = (collision.transform.position - transform.position);
        Vector2 targetPositionPlayer = (transform.position - collision.transform.position);
        targetPositionEnemy.Normalize();
        targetPositionPlayer.Normalize();
        StartCoroutine(PushBack(targetPositionEnemy, pushBackDuration, false));
        StartCoroutine(PushBack(targetPositionPlayer, pushBackDuration, true));
    }

    IEnumerator PushBack(Vector2 direction, float duration, bool isPlayer)
    {
        IdlePosition(); //this resolves some bugs
        isPushedBack = true;
        forceIdle = true;
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            float smoothSpeed;
            if (!isPlayer)
            {
                smoothSpeed = 1.0f - Mathf.Pow(1.0f - (elapsedTime / duration), 4.0f) * pushBackMagnitude;
                transform.parent.Translate(direction * smoothSpeed * Time.deltaTime);
            }
            else
            {
                smoothSpeed = 1.0f - Mathf.Pow(1.0f - (elapsedTime / duration), 4.0f) * pushBackMagnitude * pushBackMagnitudeReduceForPlayer;
                target.transform.Translate(direction * smoothSpeed * Time.deltaTime);
                Debug.Log(smoothSpeed);
            }

            if (smoothSpeed >= -1.0f) break;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isPushedBack = false;
    }
}
