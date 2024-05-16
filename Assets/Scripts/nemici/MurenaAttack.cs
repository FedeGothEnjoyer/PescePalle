using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurenaAttack : MonoBehaviour
{
    GameObject target;
    [SerializeField] float blockDuration = 5f;
    [SerializeField] float Attackcooldown = 5f;
    [SerializeField] float electricDistance = 4f;
    [SerializeField] RuntimeAnimatorController electicMurena;
    [SerializeField] RuntimeAnimatorController electicPlayer;

    private float currentTimeAttackCooldown = 0f;

    Animator animator;
    RuntimeAnimatorController animatorControllerMurena;
    SpriteRenderer playerSpriteRenderer;
    Animator playerAnimator;
    RuntimeAnimatorController playerAnimatorcontroller;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerMovement.active.gameObject;
        playerSpriteRenderer = target.transform.GetComponent<SpriteRenderer>();
        playerAnimator = target.transform.GetComponent<Animator>();
        playerAnimatorcontroller = playerAnimator.runtimeAnimatorController;
        animator = transform.GetComponent<Animator>();
        animatorControllerMurena = animator.runtimeAnimatorController;
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

        if(Vector2.Distance(transform.position, target.transform.position) <= electricDistance && !EnemyManager.forceIdleEveryone)
        {
            if(animator.runtimeAnimatorController != electicMurena)
			{
                animator.runtimeAnimatorController = electicMurena;
            }
        }
        else if(animator.runtimeAnimatorController != animatorControllerMurena)
        {
            animator.runtimeAnimatorController = animatorControllerMurena;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!PlayerMovement.isInvincible && !PlayerMovement.dashing && collision.gameObject.layer == 6 && !PlayerMovement.isInflated && currentTimeAttackCooldown >= Attackcooldown && !PlayerMovement.isAttacked)
        {
            StartCoroutine(BlockPlayer(collision.gameObject));
        }
    }

    IEnumerator BlockPlayer(GameObject player)
    {
        PlayerMovement.isAttacked = true;

        playerAnimator.runtimeAnimatorController = electicPlayer;
        InputSystem.playerInputEnabled = false;

        currentTimeAttackCooldown = 0f;
        yield return new WaitForSeconds(blockDuration);

        InputSystem.playerInputEnabled = true;
        playerAnimator.runtimeAnimatorController = playerAnimatorcontroller;

        PlayerMovement.isAttacked = false;
        PlayerMovement.active.Attack();
    }

    private void OnDestroy()
    {
        try
        {
            StopAllCoroutines();
            InputSystem.playerInputEnabled = true;
        }
        catch (System.Exception) { }
        finally
        {
            playerAnimator.runtimeAnimatorController = playerAnimatorcontroller;
        }
    }
}
