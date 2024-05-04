using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

public class InteractableBehaviour : MonoBehaviour
{
	SpriteRenderer render;
	BoxCollider2D collision;
	DialougeManager dialogue;
	Animator animator;
	[SerializeField] GameObject target;
    [SerializeField] AnimatorController notSelected;
	[SerializeField] AnimatorController selected;
	[SerializeField] UnityEvent onClickEvent;

	public Dialouge dialouge;
	private Vector2 targetPosition;

	// Start is called before the first frame update
	void Start()
    {
        animator = GetComponent<Animator>();
		collision = GetComponent<BoxCollider2D>();
        render = GetComponent<SpriteRenderer>();
		dialogue = FindObjectOfType<DialougeManager>();
    }

    private void Update()
    {
        targetPosition = target.transform.position;
        render.flipX = targetPosition.x < transform.position.x;
    }

    private void OnMouseEnter()
	{
		animator.runtimeAnimatorController = selected;
	}

	private void OnMouseExit()
	{
		animator.runtimeAnimatorController = notSelected;
	}

	public bool Clicked(Vector2 mouseCheck)
	{
		Vector3 plr_pos = InputSystem.player.transform.position;

        if (collision.bounds.Contains(mouseCheck))
		{
			RaycastHit2D ray = Physics2D.Raycast(transform.position, -(transform.position - plr_pos).normalized, 10f, LayerMask.GetMask("walls"));
			if (ray.collider != null && ray.distance < Vector2.Distance(transform.position, plr_pos))
			{
				return false;
			}
			onClickEvent.Invoke();
			InputSystem.dialogueEnabled = true;
			dialogue.StartDialouge(dialouge);
			return true;
		}
		return false;
    }
}