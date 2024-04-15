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
    [SerializeField] float dashSpeed = 8f;
    [SerializeField] float dashRange = 3f;
    bool dashing = false;

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
        if(((Vector2)transform.position - targetPos).sqrMagnitude > 0.01f)
		{
            Vector2 movement;
			if (dashing)
			{
                movement = Vector2.ClampMagnitude(Vector2.Lerp(transform.position, targetPos, dashSpeed * Time.deltaTime) - (Vector2)transform.position, dashSpeed * Time.deltaTime);

            }
            else
			{
                movement = Vector2.ClampMagnitude(Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime) - (Vector2)transform.position, maxSpeed * Time.deltaTime);
                
            }

            transform.position = transform.position + (Vector3)movement;
        }
		else
		{
            dashing = false;
		}
        
    }

	public void MoveToPosition(Vector2 target)
	{
        dashing = false;
        targetPos = target;
        render.flipX = targetPos.x < transform.position.x;

        CheckForWall();
	}

    public void MoveToObject(Vector2 target)
    {
        dashing = false;
        var vec = target-(Vector2)transform.position;
        var vec2 = Vector2.ClampMagnitude(vec, itemStopRange);
        targetPos = (Vector2)transform.position + vec - vec2;
        render.flipX = targetPos.x < transform.position.x;

        CheckForWall();
    }

    void CheckForWall()
	{
        var collider = GetComponent<CircleCollider2D>();
        RaycastHit2D ray = Physics2D.CircleCast(transform.position, collider.radius, -((Vector2)transform.position - targetPos).normalized, 10f, LayerMask.GetMask("walls"));
        if (ray.collider != null && ray.distance < ((Vector2)transform.position - targetPos).magnitude && (targetPos-ray.point).sqrMagnitude < (targetPos-(Vector2)transform.position).sqrMagnitude)
        {
            var vec = ray.point - (Vector2)transform.position;
            var vec2 = Vector2.ClampMagnitude(vec, wallStopRange);
            targetPos = (Vector2)transform.position + vec - vec2;
        }
    }

    public void Dash(Vector2 target)
	{
        var vec = target - (Vector2)transform.position;
        if(vec.magnitude > dashRange) vec = vec.normalized * dashRange;
        targetPos = vec+ (Vector2)transform.position;
        render.flipX = targetPos.x < transform.position.x;
        dashing = true;

        CheckForWall();
    }
}
