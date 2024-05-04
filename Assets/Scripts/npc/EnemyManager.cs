using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class EnemyManager : MonoBehaviour
{
    [Header("Chasing")]
    [SerializeField] GameObject target;
    [SerializeField] float scoutingRange = 1f;
    [SerializeField] float changeIdlePositionRate = 2f;
    [SerializeField] float chaseDistance = 6f;
    [Header("Pushback")]
    [SerializeField] float pushBackMagnitude = 15f;
    [SerializeField] float pushBackMagnitudeReduceForPlayer = 0.5f;
    [SerializeField] float pushBackDuration = 1f;
    [SerializeField] Sprite pushedBackEnemy;

    Seeker seeker;
    AIPath aiPath;
    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer spriteRenderer;

    public static int chasingCount; //Disables the dialouge if the player is chased by something

    private bool isChasing = false; //determines if the enemy is chasing the player, used for chasingCount
    private bool isColliding = false; //determines if enemy is colliding with the player
    private bool isPushedBack = false; //used to stop searching for idle position
    private bool forceIdle = false; //forces idle when player is inflated
    public static bool forceIdleEveryone = false; //forces idle to everyone, used after some attacks
    private float currentTimeChangeIdlePosition = 0f;

    Vector2 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        aiPath = transform.GetComponent<AIPath>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        seeker = transform.GetComponent<Seeker>();
        rigidBody = transform.GetComponent<Rigidbody2D>();
        startPosition = transform.position;
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
            if (!forceIdle && distanceToPlayer <= chaseDistance && !forceIdleEveryone)
            {
                aiPath.canSearch = true;
                if (!isChasing)
                {
                    isChasing = true;
                    chasingCount += 1;
                }
            }
            else
            {
                aiPath.canSearch = false;

                if (isChasing)
                {
                    isChasing = false;
                    chasingCount -= 1;
                }

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

    public void IdlePosition()
    {
        Vector2 randomDirection = (Random.insideUnitCircle * scoutingRange) + startPosition;
        seeker.StartPath(transform.position, randomDirection);
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
        //FIX : TODO : fare in modo che il movimento si fermi quando la vittima incontra un muro

        IdlePosition(); //this tells the enemy to return at the starting point after the pushback
        isPushedBack = true;
        forceIdle = true;
        animator.enabled = false;
        spriteRenderer.sprite = pushedBackEnemy;

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            float smoothSpeed;
            if (!isPlayer)
            {
                smoothSpeed = 1.0f - Mathf.Pow(1.0f - (elapsedTime / duration), 4.0f) * pushBackMagnitude;
                transform.Translate(direction * smoothSpeed * Time.deltaTime);
                if (IsTouchingWall(transform))
                {
                    break;
                }
            }
            else
            {
                smoothSpeed = 1.0f - Mathf.Pow(1.0f - (elapsedTime / duration), 4.0f) * pushBackMagnitude * pushBackMagnitudeReduceForPlayer;
                target.transform.Translate(direction * smoothSpeed * Time.deltaTime);
                if (IsTouchingWall(target.transform))
                {
                    break;
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        animator.enabled = true;
        isPushedBack = false;
    }

    private bool IsTouchingWall(Transform transform)
    {
        Vector2 position = transform.position;

        RaycastHit2D hitRight = Physics2D.Raycast(position, Vector2.right, 0.1f);
        if (hitRight.collider != null && hitRight.collider.gameObject.layer == 3)
        {
            return true;
        }

        RaycastHit2D hitLeft = Physics2D.Raycast(position, Vector2.left, 0.1f);
        if (hitLeft.collider != null && hitLeft.collider.gameObject.layer == 3)
        {
            return true;
        }

        RaycastHit2D hitUp = Physics2D.Raycast(position, Vector2.up, 0.1f);
        if (hitRight.collider != null && hitRight.collider.gameObject.layer == 3)
        {
            return true;
        }

        RaycastHit2D hitDown = Physics2D.Raycast(position, Vector2.down, 0.1f);
        if (hitLeft.collider != null && hitLeft.collider.gameObject.layer == 3)
        {
            return true;
        }

        return false;
    }
}
