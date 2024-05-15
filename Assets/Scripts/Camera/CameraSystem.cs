using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    GameObject target;
    [SerializeField] float followSpeed = 5f;
    public Vector2 curPos;

	private void Start()
	{
        target = PlayerMovement.active.gameObject;
        transform.position = target.transform.position;
	}

	private void Update()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.transform.position + new Vector3(0, 0, -10);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
            curPos = smoothedPosition;
        }
    }
}
