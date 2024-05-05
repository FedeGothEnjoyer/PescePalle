using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] float speed = 5f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float infl_speed_reduce = 0.4f;
    [Header("Dash")]
    [SerializeField] float dashSpeed = 8f;
    [SerializeField] float dashRange = 3f;
    [SerializeField] ParticleSystem dashEffect;
    [SerializeField] float dashFrequency = 3f;
    [Header("Inflate")]
    [SerializeField] float inflateDuration = 5f;
    [SerializeField] float inflateCooldown = 3f;
    [Header("Stop Range")]
    [SerializeField] float itemStopRange = 2f;
    public float wallStopRange = 2f;

    public bool stunned = false;


    private float dashTimer = 0f;
    private float InflateDurationTimer = 0f;
    private float InflateCooldownTimer;

    public static bool dashing = false;
    static public bool isInflated = false;

    private float startSpeed;
    private float startMaxSpeed;
    private float startDashSpeed;

    public static Vector2 targetPos;

    SpriteRenderer render;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        render = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        startSpeed = speed;
        startMaxSpeed = maxSpeed;
        startDashSpeed = dashSpeed;
        InflateCooldownTimer = inflateCooldown;
    }
    // Update is called once per frame
    void Update()
    {
		if (stunned)
		{
            dashing = false;
            targetPos = transform.position;
		}

        dashTimer += Time.deltaTime;
        InflateCooldownTimer += Time.deltaTime;

        if(isInflated) InflateDurationTimer += Time.deltaTime;

        if(InflateDurationTimer >= inflateDuration)
        {
            StopInflate();
        }

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
        if(dashTimer >= dashFrequency)
        {
            dashTimer = 0;
            var vec = target - (Vector2)transform.position;
            if (vec.magnitude > dashRange) vec = vec.normalized * dashRange;
            targetPos = vec + (Vector2)transform.position;
            render.flipX = targetPos.x < transform.position.x;
            dashing = true;
            Instantiate(dashEffect, transform.position, Quaternion.identity, transform);

            CheckForWall();
        }
    }

    public void Inflate()
    {
        if (!isInflated && InflateCooldownTimer >= inflateCooldown)
        {
            StartInflate();
        }
        else if (isInflated)
        {
            StopInflate();
        }
    }

    private void StartInflate()
    {
        isInflated = true;


        InflateCooldownTimer = 0;
        InflateDurationTimer = 0;
        animator.Play("plrAttackOpn");
        maxSpeed *= infl_speed_reduce;
        speed *= infl_speed_reduce;
        dashSpeed = 0;
    }

    private void StopInflate()
    {
        isInflated = false;

        InflateCooldownTimer = 0;
        InflateDurationTimer = 0;
        animator.Play("plrAttackExit");
        maxSpeed = startMaxSpeed;
        speed = startSpeed;
        dashSpeed = startDashSpeed;
    }
}
