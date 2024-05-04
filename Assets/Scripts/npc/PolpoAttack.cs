using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolpoAttack : MonoBehaviour
{
    [SerializeField] float blockDuration = 5f;
    [SerializeField] float Attackcooldown = 5f;
    [SerializeField] Sprite polpoBlocking;

    private static bool attacking = false;
    private float currentTimeAttackCooldown = 0f;

    AIPath aiPath;
    Animator animator;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        aiPath= transform.GetComponent<AIPath>();
        animator = transform.GetComponent<Animator>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTimeAttackCooldown < Attackcooldown)
        {
            currentTimeAttackCooldown += Time.deltaTime;
            EnemyManager.forceIdleEveryone = true;
        }
        else 
        {
            EnemyManager.forceIdleEveryone = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6 && !PlayerMovement.isInflated && currentTimeAttackCooldown >= Attackcooldown && !attacking)
        {
            StartCoroutine(BlockPlayer(collision.gameObject));
        }
    }
    
    IEnumerator BlockPlayer(GameObject player)
    {
        attacking = true;

        SpriteRenderer playerSpriteRenderer = player.transform.GetComponent<SpriteRenderer>();
        Animator playerAnimator = player.transform.GetComponent<Animator>();

        playerSpriteRenderer.enabled = false; //hide the player
        playerAnimator.enabled = false;
        animator.enabled = false;
        spriteRenderer.sprite = polpoBlocking;
        aiPath.canMove = false;

        InputSystem.playerInputEnabled = false;
        yield return new WaitForSeconds(blockDuration);
        currentTimeAttackCooldown = 0f;

        InputSystem.playerInputEnabled = true;     

        playerSpriteRenderer.enabled = true;
        playerAnimator.enabled = true;
        animator.enabled = true;
        aiPath.canMove = true;

        attacking = false;
    }
}
