using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class LanternaScript : MonoBehaviour
{
    [SerializeField] float attackDuration = 4;
    [SerializeField] float lightTransitionSpeed = 5f;
    [SerializeField] float attackCooldown = 5;
    [SerializeField] float nearAnimationDistance = 4;
    [SerializeField] RuntimeAnimatorController lanternaChaseAnimation;
    [SerializeField] Image lightOverlay;

    float currentTimeAttackCooldown;
    AIPath aiPath;
    GameObject target;
    Animator animator;
    RuntimeAnimatorController lanternaIdleAnimation;
    Color newLightColor;

    // Start is called before the first frame update
    void Start()
    {
        currentTimeAttackCooldown = 0;
        target = PlayerMovement.active.gameObject;
        aiPath = transform.GetComponent<AIPath>();
        animator = transform.GetComponent<Animator>();
        lanternaIdleAnimation = animator.runtimeAnimatorController;
        lightOverlay = GameObject.Find("LanternaLight").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTimeAttackCooldown < attackCooldown)
        {
            currentTimeAttackCooldown += Time.deltaTime;
            EnemyManager.forceIdleEveryone = true;
        }
        else
        {
            EnemyManager.forceIdleEveryone = false;
        }

        if (Vector2.Distance(transform.position, target.transform.position) <= nearAnimationDistance && !EnemyManager.forceIdleEveryone)
        {
            animator.runtimeAnimatorController = lanternaChaseAnimation;
        }
        else
        {
            animator.runtimeAnimatorController = lanternaIdleAnimation;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!PlayerMovement.isInvincible && !PlayerMovement.dashing && collision.gameObject.layer == 6 && !PlayerMovement.isInflated && currentTimeAttackCooldown >= attackCooldown && !PlayerMovement.isAttacked)
        {
            Debug.Log("Touched.");
            StopAllCoroutines();
            StartCoroutine(BlockPlayer(collision.gameObject));
        }
    }

    IEnumerator BlockPlayer(GameObject player)
    {
        PlayerMovement.isAttacked = true;

        aiPath.canMove = false;
        InputSystem.playerInputEnabled = false;

        lightOverlay.gameObject.SetActive(true);
        newLightColor = Color.white;
        newLightColor.a = 1;
        //Transizione Giallo - Bianco
        while(lightOverlay.color.a < 0.99f)
        {
            lightOverlay.color = Color.Lerp(lightOverlay.color, newLightColor, lightTransitionSpeed * Time.deltaTime);
            yield return null;
        }    

        yield return new WaitForSeconds(attackDuration);
        currentTimeAttackCooldown = 0f;

        newLightColor = Color.yellow;
        newLightColor.a = 0;
        //Transizione Bianco - Giallo
        while (lightOverlay.color.a > 0.05f)
        {
            lightOverlay.color = Color.Lerp(lightOverlay.color, newLightColor, lightTransitionSpeed * Time.deltaTime);
            yield return null;
        }
        lightOverlay.gameObject.SetActive(false);

        InputSystem.playerInputEnabled = true;
        aiPath.canMove = true;

        PlayerMovement.isAttacked = false;
        PlayerMovement.active.Attack();
 
        yield return null;
    }
}
