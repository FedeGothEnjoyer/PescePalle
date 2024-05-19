using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pescePetrolioEvent : MonoBehaviour
{
    [SerializeField] float time = 10f;
    [SerializeField] Vector2 lastPos;
    float currentTime = 0f;
    Vector2 startPos;

	private void Start()
	{
        startPos = transform.position;
    }

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 6) return;
        if (CurrentData.pescePetrolioEvent)
        {
            Destroy(gameObject);
        }
        else
        {
            startPos = transform.position;
            StartCoroutine(Animation());
            Camera.main.GetComponent<CameraShake>().Shake(time);
            GetComponent<AudioSource>().Play();
        }
        CurrentData.pescePetrolioEvent = true;
    }

    IEnumerator Animation()
    {
        //InputSystem.playerInputEnabled = false;
        while(currentTime < time)
		{
            currentTime += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, lastPos, currentTime / time);
            yield return null;
        }
        //InputSystem.playerInputEnabled = true;
    }
}
