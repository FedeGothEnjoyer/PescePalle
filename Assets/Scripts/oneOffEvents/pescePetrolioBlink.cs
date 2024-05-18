using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pescePetrolioBlink : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 6) return;
        if (CurrentData.pescePetrolioBlink)
        {
            Destroy(gameObject);
        }
        else
        {
            animator.speed = 1;
            animator.Play(animator.runtimeAnimatorController.animationClips[0].name);
            StartCoroutine(FadeToBlack());
        }
        CurrentData.pescePetrolioBlink = true;
    }

    IEnumerator FadeToBlack()
	{
        while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
		{
            yield return null;
		}
        Color c = new Color(1f, 1f, 1f, 0f);
        var sprite = GetComponent<SpriteRenderer>();
        while(sprite.color.a > 0.05f)
		{
            sprite.color = Color.Lerp(sprite.color, c, Time.deltaTime);
            yield return null;
		}
        sprite.color = c;
    }
}
