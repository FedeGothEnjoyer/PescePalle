using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float itemStopRange = 2f;
    [SerializeField] float wallStopRange = 2f;
    [SerializeField] Vector2 targetPos;

    SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if((Vector2)transform.position != targetPos)
		{
            var movement = Vector2.ClampMagnitude(Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime) - (Vector2)transform.position, maxSpeed * Time.deltaTime);
            transform.position = transform.position + (Vector3)movement;
        }
        
    }

	public void MoveToPosition(Vector2 target)
	{
        targetPos = target;
        render.flipX = targetPos.x < transform.position.x;

        CheckForWall();
	}

    public void MoveToObject(Vector2 target)
    {
        var vec = target-(Vector2)transform.position;
        var vec2 = Vector2.ClampMagnitude(vec, itemStopRange);
        targetPos = (Vector2)transform.position + vec - vec2;
        render.flipX = targetPos.x < transform.position.x;

        CheckForWall();
    }

    void CheckForWall()
	{
        RaycastHit2D ray = Physics2D.Raycast(transform.position, -((Vector2)transform.position - targetPos).normalized, 10f, LayerMask.GetMask("walls"));
        if (ray.collider != null && ray.distance < ((Vector2)transform.position - targetPos).magnitude)
        {
            var vec = ray.point - (Vector2)transform.position;
            var vec2 = Vector2.ClampMagnitude(vec, wallStopRange);
            targetPos = (Vector2)transform.position + vec - vec2;
        }
    }
}
