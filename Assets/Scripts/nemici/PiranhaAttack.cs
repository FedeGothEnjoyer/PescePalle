using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PiranhaAttack : MonoBehaviour
{
    [SerializeField] float NearAnimationDistance = 4f;
    [SerializeField] float slowDuration = 8f;
    [SerializeField] float speedReducer = 0.35f;
    [SerializeField] float Attackcooldown = 6f;
    [SerializeField] RuntimeAnimatorController piranhaBite;
    [SerializeField] RuntimeAnimatorController piranhaNear;
    [SerializeField] AudioClip biteSound;

    private float currentTimeAttackCooldown = 0f;

    GameObject target;
    AIPath aiPath;
    Animator animator;
    Animator playerAnimator;
    RuntimeAnimatorController playerAnimatorController;
    SpriteRenderer spriteRenderer;
    RuntimeAnimatorController animatorController;
    AudioSource audioSource;

    float playerSpeed;
    float playerMaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerMovement.active.gameObject;
        audioSource = transform.GetComponent<AudioSource>();
        playerSpeed = target.GetComponent<PlayerMovement>().speed;
        playerMaxSpeed = target.GetComponent<PlayerMovement>().maxSpeed;
        playerAnimator = target.GetComponent<Animator>();
        playerAnimatorController = playerAnimator.runtimeAnimatorController;
        aiPath = transform.GetComponent<AIPath>();
        animator = transform.GetComponent<Animator>();
        animatorController = animator.runtimeAnimatorController;
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

        if (Vector2.Distance(transform.position, target.transform.position) <= NearAnimationDistance && animator.runtimeAnimatorController != piranhaBite)
        {
            animator.runtimeAnimatorController = piranhaNear;
        }
        else if (animator.runtimeAnimatorController != animatorController)
        {
            animator.runtimeAnimatorController = animatorController;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (!PlayerMovement.isInvincible && !PlayerMovement.dashing && collision.gameObject.layer == 6 && !PlayerMovement.isInflated && currentTimeAttackCooldown >= Attackcooldown && !PlayerMovement.isAttacked)
        {
            StartCoroutine(SlowPlayer(collision.gameObject));
        }
    }

    IEnumerator SlowPlayer(GameObject player)
    {
        PlayerMovement.isAttacked = true;

        audioSource.clip = biteSound;
        audioSource.Play();

        SpriteRenderer playerSpriteRenderer = player.transform.GetComponent<SpriteRenderer>();
        Animator playerAnimator = player.transform.GetComponent<Animator>();
        //Cambio sprite e animator
        animator.enabled = false;
        spriteRenderer.enabled = false;
        playerAnimator.runtimeAnimatorController = piranhaBite;

        aiPath.canMove = false;
        //Rallentamento
        InputSystem.inflateEnabled = false;
        InputSystem.dashEnabled = false;
        player.GetComponent<PlayerMovement>().speed *= speedReducer;
        player.GetComponent<PlayerMovement>().maxSpeed *= speedReducer;
        //Attesa
        yield return new WaitForSeconds(slowDuration);
        currentTimeAttackCooldown = 0f;

        InputSystem.inflateEnabled = true;
        InputSystem.dashEnabled = true;
        player.GetComponent<PlayerMovement>().speed = playerSpeed;
        player.GetComponent<PlayerMovement>().maxSpeed = playerMaxSpeed;

        aiPath.canMove = false;
        transform.position = player.transform.position;

        animator.enabled = true;
        spriteRenderer.enabled = true;
        playerAnimator.runtimeAnimatorController = playerAnimatorController;


        PlayerMovement.isAttacked = false;
        PlayerMovement.active.Attack();
    }

    private void OnDestroy()
    {
        try
        {
            StopAllCoroutines();
            InputSystem.inflateEnabled = true;
            InputSystem.dashEnabled = true;
            target.GetComponent<PlayerMovement>().speed = playerSpeed;
            target.GetComponent<PlayerMovement>().maxSpeed = playerMaxSpeed;
            playerAnimator.runtimeAnimatorController = playerAnimatorController;
            PlayerMovement.isAttacked = false;
        }
        catch(System.Exception) { }
    }
}
