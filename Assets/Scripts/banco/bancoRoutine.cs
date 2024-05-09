using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bancoRoutine : MonoBehaviour
{
    public float speed;
    public bool flipped;
    public float range;

    void Update()
    {
        if(flipped) transform.position = transform.position - (Vector3)(Vector2.right * speed * Time.deltaTime);
        else transform.position = transform.position + (Vector3)(Vector2.right * speed * Time.deltaTime);

        if (transform.position.x > range && !flipped) Destroy(gameObject);
        if (transform.position.x < -range && flipped) Destroy(gameObject);
    }
}
