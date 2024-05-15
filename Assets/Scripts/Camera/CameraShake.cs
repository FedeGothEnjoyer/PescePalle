using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    float duration = 1f;
    static public CameraShake instance;

	private void Awake()
	{
        if (instance != null) Destroy(gameObject);
        else instance = this;
	}

	public void Shake(float time)
	{
        duration = time;
        StartCoroutine(nameof(Shaking));
	}

    IEnumerator Shaking()
    {
        
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            Vector3 startPosition = GetComponent<CameraSystem>().curPos;
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + (Vector3)Random.insideUnitCircle * strength;
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            yield return null;
        }
        transform.position = new Vector3(GetComponent<CameraSystem>().curPos.x, GetComponent<CameraSystem>().curPos.y, -10);
    }
}