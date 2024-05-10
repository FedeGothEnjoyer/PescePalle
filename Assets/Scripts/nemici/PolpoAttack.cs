using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolpoAttack : MonoBehaviour
{
    [SerializeField] float blockDuration = 5f;
    [SerializeField] float Attackcooldown = 5f;
    [SerializeField] RuntimeAnimatorController polpoBlocking;

    private float currentTimeAttackCooldown = 0f;

    AIPath aiPath;
    Animator animator;
    SpriteRenderer spriteRenderer;
    RuntimeAnimatorController animatorController;
    MessageManager messageManager;

    // Start is called before the first frame update
    void Start()
    {
        aiPath = transform.GetComponent<AIPath>();
        animator = transform.GetComponent<Animator>();
        animatorController = animator.runtimeAnimatorController;
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        messageManager = MessageManager.instance;
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
        if(!PlayerMovement.isInvincible && !PlayerMovement.dashing && collision.gameObject.layer == 6 && !PlayerMovement.isInflated && currentTimeAttackCooldown >= Attackcooldown && !PlayerMovement.isAttacked)
        {
            StartCoroutine(BlockPlayer(collision.gameObject));
        }
    }
    
    IEnumerator BlockPlayer(GameObject player)
    {
        messageManager.StartMessage("hah COGLIONE blub blub blub", 2);
        PlayerMovement.isAttacked = true;

        SpriteRenderer playerSpriteRenderer = player.transform.GetComponent<SpriteRenderer>();
        Animator playerAnimator = player.transform.GetComponent<Animator>();

        playerSpriteRenderer.enabled = false; //hide the player
        playerAnimator.enabled = false;
        animator.runtimeAnimatorController = polpoBlocking;
        aiPath.canMove = false;

        InputSystem.playerInputEnabled = false;
        yield return new WaitForSeconds(blockDuration);
        currentTimeAttackCooldown = 0f;

        InputSystem.playerInputEnabled = true;     

        playerSpriteRenderer.enabled = true;
        playerAnimator.enabled = true;
        animator.runtimeAnimatorController = animatorController;
        aiPath.canMove = true;

        PlayerMovement.isAttacked = false;
        PlayerMovement.active.Attack();
    }
}
