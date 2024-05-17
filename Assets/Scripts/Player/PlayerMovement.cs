using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] public float speed = 5f;
    [SerializeField] public float maxSpeed = 5f;
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
    [SerializeField] float wallDistancingSpeed;
    [Header("Invincibility")]
    [SerializeField] float invincibleTime = 1f;

    public static List<Vector2> foodPositions;

    public bool stunned = false;
    public static bool isAttacked;
    public static bool isInvincible;
    private float invincibleTimer;

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
    static public int transitionStage = 0;
    private float transitionTimer;
    [SerializeField] private bool newDayAnimation;
    private bool firstLoad = true;
    private bool destructionPass = false;

    [Header("Sounds")]
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip inflateSound;

    public static Vector2 targetPos;

    SpriteRenderer render;
    Animator animator;
    AudioSource audioSource;

    public static PlayerMovement active;

    private void Awake()
    {
        //singleton check
        DontDestroyOnLoad(gameObject);
        if (active != null && active != this) DestroyImmediate(gameObject);
        else
        {
            active = this;
            DialougeManager.instance = GetComponentInChildren<DialougeManager>();
            render = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            transitionStage = 2;
            transitionTimer = 0.5f * transitionDuration;
            firstLoad = true;
            foodPositions = new List<Vector2>();
            CurrentData.shipFallen = false;
            newDayText.text = "GIORNO " + CurrentData.day;
            FoodManager.foodTaken = 0;
        }
    }


    void Start()
    {
        Screen.SetResolution(1152, 640, true);

        isInvincible = false;
        invincibleTimer = 0f;
        isAttacked = false;
        targetPos = transform.position;
        
        startSpeed = speed;
        startMaxSpeed = maxSpeed;
        startDashSpeed = dashSpeed;
        InflateCooldownTimer = inflateCooldown;
        foreach(Canvas c in GetComponentsInChildren<Canvas>()) c.worldCamera = Camera.main;
        
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;
            if (invincibleTimer >= invincibleTime)
            {
                isInvincible = false;
            }
        }
        if (transitionStage != 0)
        {
            if (destructionPass)
            {
                foreach (FoodScript food in FindObjectsByType<FoodScript>(FindObjectsSortMode.InstanceID))
                {
                    foreach (Vector2 v in foodPositions)
                    {
                        if ((int)v.x == (int)food.transform.position.x && (int)v.y == (int)food.transform.position.y) DestroyImmediate(food.gameObject);
                    }
                }
                foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
                {
                    p.Clear();
                }
                destructionPass = false;
            }
            transitionTimer += Time.deltaTime;
            if (newDayAnimation || firstLoad) transitionTimer -= Time.deltaTime * 0.75f;
            Color color = blackFade.color;
            color.a = fadeCurve.Evaluate(transitionTimer / transitionDuration);
            if (newDayAnimation && (SceneManager.GetActiveScene().name != "Tuto" || c_targetScene == "Tana"))
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
                firstLoad = false;
            }
            blackFade.color = color;

            if (transitionTimer / transitionDuration > 0.5f && transitionStage == 1)
            {
                EnemyManager.chasingCount = 0;
                isAttacked = false;
                isInflated = false;
                active.Attack(); //trigger invincibility
                transitionStage = 2;
                render.flipX = c_flipped;
                if (c_targetScene != null) SceneManager.LoadScene(c_targetScene);
                transform.position = c_spawnPos;
                targetPos = c_spawnPos;
                Start();
                destructionPass = true;
            }


            return;
        }

        DistanceWalls();

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
        RaycastHit2D ray = Physics2D.CircleCast(transform.position, collider.radius*0.5f, -((Vector2)transform.position - targetPos).normalized, 10f, LayerMask.GetMask("walls"));
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
            audioSource.clip = dashSound;
            audioSource.Play();
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
        audioSource.clip = inflateSound;
        audioSource.Play();

        InflateCooldownTimer = 0;
        InflateDurationTimer = 0;
        animator.Play("plrAttackOpn");
        maxSpeed *= infl_speed_reduce;
        speed *= infl_speed_reduce;
        dashSpeed = 0;
    }

    private void StopInflate()
    {
        if(isInflated) animator.Play("plrAttackExit");
        isInflated = false;

        InflateCooldownTimer = 0;
        InflateDurationTimer = 0;
        
        maxSpeed = startMaxSpeed;
        speed = startSpeed;
        dashSpeed = startDashSpeed;
    }


    //porcata galattica warning
    Vector2 c_spawnPos;
    bool c_flipped;
    string c_targetScene;

    public void ChangingRoom(Vector2 spawnPos, bool flipped, string targetScene, bool versoTana, string newDayTarget)
    {
        if (transitionStage != 0) return;
        transitionTimer = 0;
        transitionStage = 1;
        c_spawnPos = spawnPos;
        c_flipped = flipped;
        c_targetScene = targetScene;
        StopInflate();
        if(versoTana && FoodManager.foodTaken >= 2)
		{
            newDayAnimation = true;
            CurrentData.day++;
            newDayText.text = "GIORNO " + CurrentData.day;
            FoodManager.foodTaken = 0;
            c_targetScene = newDayTarget;
            foodPositions = new List<Vector2>();
        }
    }

    private void DistanceWalls()
	{
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, 0.7f, LayerMask.GetMask("walls"));
        if (ray.collider != null)
        {
            targetPos = new Vector2(targetPos.x, Mathf.Lerp(targetPos.y, targetPos.y+1f, wallDistancingSpeed * Time.deltaTime));
        }
        ray = Physics2D.Raycast(transform.position, Vector2.left, 0.7f, LayerMask.GetMask("walls"));
        if (ray.collider != null)
        {
            targetPos = new Vector2(Mathf.Lerp(targetPos.x, targetPos.x + 1f, wallDistancingSpeed * Time.deltaTime), targetPos.y);
        }
        ray = Physics2D.Raycast(transform.position, Vector2.right, 0.7f, LayerMask.GetMask("walls"));
        if (ray.collider != null)
        {
            targetPos = new Vector2(Mathf.Lerp(targetPos.x, targetPos.x - 1f, wallDistancingSpeed * Time.deltaTime), targetPos.y);
        }
        ray = Physics2D.Raycast(transform.position, Vector2.up, 0.7f, LayerMask.GetMask("walls"));
        if (ray.collider != null)
        {
            targetPos = new Vector2(targetPos.x, Mathf.Lerp(targetPos.y, targetPos.y - 1f, wallDistancingSpeed * Time.deltaTime));
        }
    }

    public void Attack()
	{
        isInvincible = true;
        invincibleTimer = 0f;
	}
}
