using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableBehaviour : MonoBehaviour
{
	SpriteRenderer render;
	BoxCollider2D collision;
	DialougeManager dialogue;
	[SerializeField] Sprite notSelected;
	[SerializeField] Sprite selected;
	[SerializeField] UnityEvent onClickEvent;

	public Dialouge dialouge;

	// Start is called before the first frame update
	void Start()
    {
		collision = GetComponent<BoxCollider2D>();
        render = GetComponent<SpriteRenderer>();
		dialogue = FindObjectOfType<DialougeManager>();
    }

	private void OnMouseEnter()
	{
		render.sprite = selected;
	}

	private void OnMouseExit()
	{
		render.sprite = notSelected;
	}

	public bool Clicked(Vector2 mouseCheck)
	{
		if (collision.bounds.Contains(mouseCheck))
		{
			RaycastHit2D ray = Physics2D.Raycast(transform.position, -(transform.position - InputSystem.player.transform.position).normalized, 10f, LayerMask.GetMask("walls"));
			if (ray.collider != null && ray.distance < Vector2.Distance(transform.position, InputSystem.player.transform.position))
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
