using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public GameObject target;
    public float followSpeed = 5f;

    private void Update()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.transform.position + new Vector3(0, 0, -10);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
