using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("Transition")]
    [SerializeField] SpriteRenderer blackFade;
    [SerializeField] AnimationCurve fadeCurve;
    [SerializeField] AnimationCurve textCurve;
    [SerializeField] float transitionDuration;
    [SerializeField] Text newDayText;
    private int transitionStage = 0;
    private float transitionTimer;
    [SerializeField] private bool newDayAnimation;

    public static Vector2 targetPos;

    SpriteRenderer render;
    Animator animator;

    public static PlayerMovement active;

    private void Awake()
    {
        //singleton check
        DontDestroyOnLoad(this);
        if (active != null && active != this) DestroyImmediate(gameObject);
        else active = this;
    }


    void Start()
    {
        targetPos = transform.position;
        render = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        startSpeed = speed;
        startMaxSpeed = maxSpeed;
        startDashSpeed = dashSpeed;
        InflateCooldownTimer = inflateCooldown;
        foreach(Canvas c in GetComponentsInChildren<Canvas>()) c.worldCamera = Camera.main;
    }

    void Update()
    {
        if (transitionStage != 0)
        {
            transitionTimer += Time.deltaTime;
            if(newDayAnimation) transitionTimer -= Time.deltaTime * 0.75f;
            Color color = blackFade.color;
            color.a = fadeCurve.Evaluate(transitionTimer / transitionDuration);
			if (newDayAnimation)
			{
                Color colorT = newDayText.color;
                colorT.a = textCurve.Evaluate(transitionTimer / transitionDuration);
                newDayText.color = colorT;
            }
            if (transitionTimer / transitionDuration > 0.95f)
            {
                transitionStage = 0;
                color.a = 0;
                newDayAnimation = false;
                Start();
            }
            blackFade.color = color;

            if (transitionTimer / transitionDuration > 0.5f && transitionStage == 1)
            {
                transitionStage = 2;
                render.flipX = c_flipped;
                SceneManager.LoadScene(c_targetScene.name);
                transform.position = c_spawnPos;
                targetPos = c_spawnPos;
                Start();
            }


            return;
        }

        if (stunned)
        {
            dashing = false;
            targetPos = transform.position;
        }

        dashTimer += Time.deltaTime;
        InflateCooldownTimer += Time.deltaTime;

        if (isInflated) InflateDurationTimer += Time.deltaTime;

        if (InflateDurationTimer >= inflateDuration)
        {
            StopInflate();
        }

        if (((Vector2)transform.position - targetPos).sqrMagnitude > 0.01f)
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
        var vec = target - (Vector2)transform.position;
        var vec2 = Vector2.ClampMagnitude(vec, itemStopRange);
        targetPos = (Vector2)transform.position + vec - vec2;
        render.flipX = targetPos.x < transform.position.x;

        CheckForWall();
    }

    void CheckForWall()
    {
        var collider = GetComponent<CircleCollider2D>();
        RaycastHit2D ray = Physics2D.CircleCast(transform.position, collider.radius, -((Vector2)transform.position - targetPos).normalized, 10f, LayerMask.GetMask("walls"));
        if (ray.collider != null && ray.distance < ((Vector2)transform.position - targetPos).magnitude && (targetPos - ray.point).sqrMagnitude < (targetPos - (Vector2)transform.position).sqrMagnitude)
        {
            var vec = ray.point - (Vector2)transform.position;
            var vec2 = Vector2.ClampMagnitude(vec, wallStopRange);
            targetPos = (Vector2)transform.position + vec - vec2;
        }
    }

    public void Dash(Vector2 target)
    {
        if (dashTimer >= dashFrequency)
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


    //porcata galattica warning
    Vector2 c_spawnPos;
    bool c_flipped;
    SceneAsset c_targetScene;

    public void ChangingRoom(Vector2 spawnPos, bool flipped, SceneAsset targetScene, bool versoTana)
    {
        transitionTimer = 0;
        transitionStage = 1;
        c_spawnPos = spawnPos;
        c_flipped = flipped;
        c_targetScene = targetScene;
        if(versoTana && FoodManager.foodTaken >= 2)
		{
            newDayAnimation = true;
            CurrentData.day++;
            newDayText.text = "GIORNO " + CurrentData.day;
            FoodManager.foodTaken = 0;
        }
    }
}
