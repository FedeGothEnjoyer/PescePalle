using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipLightBehaviour : MonoBehaviour
{
    [SerializeField] float minAngle;
    [SerializeField] float maxAngle;
	[SerializeField] float duration;

	private void Update()
	{
		transform.rotation = Quaternion.Euler(0, 0, (Mathf.Sin(Time.time/duration)+1f)/2*(maxAngle-minAngle)+minAngle);
	}
}
