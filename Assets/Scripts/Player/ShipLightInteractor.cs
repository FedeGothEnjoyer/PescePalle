using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipLightInteractor : MonoBehaviour
{
	[SerializeField] AudioClip spottedSound;
	[SerializeField] int lightNumber;
	[SerializeField] int shadowNumber;
	SpriteMask mask;

	private void Start()
	{
		Enable(new Scene(), LoadSceneMode.Single);
		
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
		SceneManager.sceneLoaded += Enable;
		SceneManager.sceneUnloaded += Disable;
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
		PlayerMovement.active.GetComponent<AudioSource>().clip = spottedSound;
        PlayerMovement.active.GetComponent<AudioSource>().volume = 0.2f;
        PlayerMovement.active.GetComponent<AudioSource>().Play();
        GetComponent<PlayerMovement>().ChangingRoom(new Vector2(-20f, 0.34f), false, SceneManager.GetActiveScene().name, false, "");
	}

	private void ValueReset(Scene s1, Scene s2)
	{
		lightNumber = 0;
		shadowNumber = 0;
	}

	private void Enable(Scene s, LoadSceneMode l)
	{
		if (FindFirstObjectByType<shipLightBehaviour>() != null)
		{
			mask = PlayerMovement.active.GetComponentInChildren<SpriteMask>();
			mask.enabled = false;
		}
	}

	private void Disable(Scene s)
	{
		if(mask != null) mask.enabled = true;
	}
}
