using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableBehaviour : MonoBehaviour
{
	SpriteRenderer render;
	BoxCollider2D collision;
	DialougeManager dialogue;
	Animator animator;
	GameObject target;
    RuntimeAnimatorController notSelected;
	[SerializeField] AnimatorOverrideController selected;
	[SerializeField] UnityEvent onClickEvent;
	[SerializeField] float distanceForSelect = 200f;
	[SerializeField] bool triggerOnLoad = false;
	private bool first = true;

	public Dialouge dialouge;
	private Vector2 targetPosition;
	bool isSelected;

	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
		collision = GetComponent<BoxCollider2D>();
		render = GetComponent<SpriteRenderer>();
		target = PlayerMovement.active.gameObject;
		dialogue = GameObject.Find("DialougeManager").GetComponent<DialougeManager>();
		notSelected = animator.runtimeAnimatorController;
		first = true;
	}

    private void Update()
    {
		if(triggerOnLoad && first)
		{
			InputSystem.dialogueEnabled = true;
			dialogue.StartDialouge(dialouge);
			InputSystem.enemyMovementEnabled = false;
			InputSystem.playerInputEnabled = false;
			first = false;
			dialogue.DisplayNextSentence();
		}

        targetPosition = target.transform.position;
        render.flipX = targetPosition.x < transform.position.x;

        float distanceToPlayer = ((Vector2)transform.position - targetPosition).sqrMagnitude;
		if(isSelected || distanceToPlayer * distanceToPlayer <= distanceForSelect)
            animator.runtimeAnimatorController = selected;
		else
            animator.runtimeAnimatorController = notSelected;
    }

    private void OnMouseEnter()
	{
		if (InputSystem.selectedInteractableEnabled)
		{
			isSelected = true;
            animator.runtimeAnimatorController = selected;
        }
	}

	private void OnMouseExit()
	{
		animator.runtimeAnimatorController = notSelected;
		isSelected = false;
	}

	public bool Clicked(Vector2 mouseCheck)
	{
		Vector3 plr_pos = InputSystem.player.transform.position;
		if (InputSystem.dialogueEnabled) return false;

        if (collision.bounds.Contains(mouseCheck))
		{
			RaycastHit2D ray = Physics2D.Raycast(transform.position, -(transform.position - plr_pos).normalized, 10f, LayerMask.GetMask("walls"));
			if (ray.collider != null && ray.distance < Vector2.Distance(transform.position, plr_pos))
			{
				return false;
			}
			onClickEvent.Invoke();
			dialogue = DialougeManager.instance;
			dialogue.StartDialouge(dialouge);
			InputSystem.dialogueEnabled = true;
			
			return true;
		}
		return false;
    }
}
