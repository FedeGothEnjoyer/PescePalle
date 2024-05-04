using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bancoRoutine : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.position = transform.position + (Vector3)(Vector2.right * speed * Time.deltaTime);
        if (transform.position.x > 10f) Destroy(gameObject);
    }
}
