using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyManager : MonoBehaviour
{
    [Header("Chasing")]
    GameObject target;
    [SerializeField] float scoutingRange = 1f;
    [SerializeField] float changeIdlePositionRate = 2f;
    [SerializeField] float chaseDistance = 6f;
    [SerializeField] float maxDistanceFromStart = 15f;
    [Header("Pushback")]
    [SerializeField] float pushBackMagnitude = 15f;
    [SerializeField] float pushBackMagnitudeReduceForPlayer = 0.5f;
    [SerializeField] float pushBackDurationPlayer = 1f;
    [SerializeField] float pushBackDurationEnemy = 1f;
    [SerializeField] Sprite pushedBackEnemy;

    Seeker seeker;
    AIPath aiPath;
    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer spriteRenderer;

    public static int chasingCount; //Disables the dialouge if the player is chased by something

    private bool isChasing = false; //determines if the enemy is chasing the player, used for chasingCount
    private bool isColliding = false; //determines if enemy is colliding with the player
    public bool isPushedBack = false; //used to stop searching for idle position
    private bool forceIdle = false; //forces idle when player is inflated
    public static bool forceIdleEveryone = false; //forces idle to everyone, used after some attacks
    private float currentTimeChangeIdlePosition = 0f;

    Vector2 startPosition;
    public Vector2 targetPositionEnemy;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerMovement.active.gameObject;
        aiPath = transform.GetComponent<AIPath>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        seeker = transform.GetComponent<Seeker>();
        rigidBody = transform.GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        aiPath.canSearch = false;
        GetComponent<AIDestinationSetter>().target = target.transform;
    }

    // Update is called once per frame
    void Update()
    {
        targetPositionEnemy = (target.transform.position - transform.position);
        targetPositionEnemy.Normalize();

        if (InputSystem.enemyMovementEnabled)
        {
            aiPath.canMove = true;
            float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
            float distanceToStart = ((Vector2)transform.position - startPosition).sqrMagnitude;
            //chase player
            if ((!forceIdle && distanceToPlayer <= chaseDistance && (!forceIdleEveryone || gameObject.tag == "piranha")) && distanceToStart <= maxDistanceFromStart * maxDistanceFromStart)
            {
                spriteRenderer.flipX = target.transform.position.x > transform.position.x;

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
                    chasingCount = Mathf.Max(0, chasingCount-1);
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
        spriteRenderer.flipX = randomDirection.x > transform.position.x;
        seeker.StartPath(transform.position, randomDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && PlayerMovement.isInflated)
            StartPushBack(targetPositionEnemy, pushBackDurationEnemy, pushBackDurationPlayer, pushBackMagnitude, pushBackMagnitudeReduceForPlayer, false);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && PlayerMovement.isInflated && !isColliding)
            StartPushBack(targetPositionEnemy, pushBackDurationEnemy, pushBackDurationPlayer, pushBackMagnitude, pushBackMagnitudeReduceForPlayer, false);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isColliding = false;
        }
    }

    public void StartPushBack(Vector2 direction, float playerDuration, float enemyDuration, float magnitude, float playerMultiplier, bool isAttack)
    {
        isColliding = true;
        StartCoroutine(PushBack(direction, playerDuration, enemyDuration, magnitude, playerMultiplier, false));
    }

    IEnumerator PushBack(Vector2 direction, float EnemyDuration, float PlayerDuration , float magnitude, float playerMultiplier, bool isAttack)
    {
        target.GetComponent<PlayerMovement>().stunned = true;
        target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        target.GetComponent<Rigidbody2D>().velocity = direction * playerMultiplier * magnitude;
        isPushedBack = true;
        animator.enabled = false;
        spriteRenderer.sprite = pushedBackEnemy;

        float elapsedTime = 0.0f;
        bool flag = false;
        while (elapsedTime < Mathf.Max(EnemyDuration, PlayerDuration))
        {
            if (elapsedTime >= PlayerDuration && !flag)
            {
                flag = true;

                target.GetComponent<PlayerMovement>().stunned = false;
                target.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
            if (elapsedTime < EnemyDuration)
            {
                float smoothSpeed;

                smoothSpeed = 1.0f - Mathf.Pow(1.0f - (elapsedTime / EnemyDuration), 3.0f) * magnitude;
                transform.Translate(direction * smoothSpeed * Time.deltaTime);
                if (IsTouchingWall(transform))
                {
                    break;
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.enabled = true;
        isPushedBack = false;

        if (!isAttack)
        {
            IdlePosition(); //this tells the enemy to return at the starting point after the pushback
            forceIdle = true;
        }
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
