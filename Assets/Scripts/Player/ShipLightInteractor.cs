using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipLightInteractor : MonoBehaviour
{
	[SerializeField] AudioClip spottedSound;
	[SerializeField] int lightNumber;
	[SerializeField] int shadowNumber;

	private void Start()
	{
		lightNumber = 0;
		shadowNumber = 0;
		SceneManager.activeSceneChanged += ValueReset;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("shipLight"))
		{
			lightNumber++;
			if (lightNumber > 0 && shadowNumber == 0) Spotted();
		}
		if (collision.CompareTag("shadow"))
		{
			shadowNumber++;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("shipLight"))
		{
			lightNumber--;
		}
		if (collision.CompareTag("shadow"))
		{
			shadowNumber--;
			if (lightNumber > 0 && shadowNumber == 0) Spotted();
		}
	}

	private void Spotted()
	{
		Camera.main.GetComponent<AudioSource>().clip = spottedSound;
		Camera.main.GetComponent<AudioSource>().Play();
		GetComponent<PlayerMovement>().ChangingRoom(new Vector2(-20f, 0.34f), false, SceneManager.GetActiveScene().name, false, "");
	}

	private void ValueReset(Scene s1, Scene s2)
	{
		lightNumber = 0;
		shadowNumber = 0;
	}
}
