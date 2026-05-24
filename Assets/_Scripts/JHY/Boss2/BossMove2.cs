using UnityEngine;

public class BossMove2 : MonoBehaviour
{

    public Transform player;
    public float moveSpeed = 2f;
    public float followRange = 10f;
    public float keepDistance = 3f;

    [Header("Dash")]
    public float dashInterval = 10f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.5f;
    public float dashPrepareTime = 0.75f;

    [Header("Dash Warning")]
    public GameObject dashWarningPrefab;
    public float dashWarningDistance = 2f;
    public bool rotateWarningToDashDirection = true;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Health health;

    private float lastDashTime;
    private float dashTimer;
    private float dashPrepareTimer;

    private bool isPreparingDash;
    private bool isDashing;

    private Vector2 dashDir;
    private GameObject currentDashWarning;

    public bool IsDashingNow => isPreparingDash || isDashing;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        lastDashTime = Time.time;

        player = NKY_GameManager.instance.player.transform;
    }

    private void Update()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Speed", 0f);
            return;
        }

        if (isPreparingDash)
        {
            dashPrepareTimer -= Time.deltaTime;
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Speed", 0f);

            if (dashPrepareTimer <= 0f)
            {
                BeginDash();
            }

            return;
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            anim.SetFloat("Speed", dashSpeed);

            if (dashTimer <= 0f)
            {
                EndDash();
            }

            return;
        }

        UpdateFacing();

        if (Time.time >= lastDashTime + dashInterval)
        {
            StartDashPrepare();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        float currentSpeed = 0f;

        if (distance <= followRange && distance > keepDistance)
        {
            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = dir * moveSpeed;
            currentSpeed = moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        anim.SetFloat("Speed", currentSpeed);
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = dashDir * dashSpeed;
        }
    }

    private void StartDashPrepare()
    {
        lastDashTime = Time.time;
        isPreparingDash = true;
        dashPrepareTimer = dashPrepareTime;
        dashDir = ((Vector2)player.position - (Vector2)transform.position).normalized;

        rb.linearVelocity = Vector2.zero;
        health?.SetDamageable(false);
        SpawnDashWarning();
    }

    private void BeginDash()
    {
        isPreparingDash = false;
        isDashing = true;
        dashTimer = dashDuration;
        RemoveDashWarning();
    }

    private void EndDash()
    {
        isDashing = false;
        rb.linearVelocity = Vector2.zero;
        anim.SetFloat("Speed", 0f);
        health?.SetDamageable(true);
    }

    private void SpawnDashWarning()
    {
        RemoveDashWarning();

        if (dashWarningPrefab == null)
        {
            return;
        }

        Vector3 spawnPos = transform.position + (Vector3)(dashDir * dashWarningDistance);
        currentDashWarning = Instantiate(dashWarningPrefab, spawnPos, Quaternion.identity);

        if (rotateWarningToDashDirection)
        {
            float angle = Mathf.Atan2(dashDir.y, dashDir.x) * Mathf.Rad2Deg;
            currentDashWarning.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void RemoveDashWarning()
    {
        if (currentDashWarning != null)
        {
            Destroy(currentDashWarning);
            currentDashWarning = null;
        }
    }

    private void UpdateFacing()
    {
        if (sr == null || player == null) return;

        if (player.position.x < transform.position.x)
            sr.flipX = false;
        else
            sr.flipX = true;
    }
}