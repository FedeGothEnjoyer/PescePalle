using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBehaviour : MonoBehaviour
{
	SpriteRenderer render;
	[SerializeField] Sprite notSelected;
	[SerializeField] Sprite selected;

	public Dialouge dialouge;

	// Start is called before the first frame update
	void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

	private void OnMouseEnter()
	{
		render.sprite = selected;
	}

	private void OnMouseExit()
	{
		render.sprite = notSelected;
	}

	private void OnMouseDown()
	{
        FindAnyObjectByType<DialougeManager>().StartDialouge(dialouge);
    }
}
