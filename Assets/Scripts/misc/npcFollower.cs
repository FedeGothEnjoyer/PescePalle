using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcFollower : MonoBehaviour
{
    [SerializeField] float followDistance;
    [SerializeField] float followSpeed;
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerMovement.active.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 distance = transform.position - target.position;
        if(distance.x*distance.x+distance.y*distance.y > followDistance * followDistance)
		{
            transform.position = Vector2.Lerp(transform.position, target.position, Time.deltaTime * followSpeed);
		}
        GetComponent<SpriteRenderer>().flipX = (distance.x < 0);
    }
}
