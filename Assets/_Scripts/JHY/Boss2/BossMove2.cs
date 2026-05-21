using UnityEngine;
public class BossFollow2D : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float followRange = 10f;
    public float keepDistance = 3f;
    public float dashInterval = 10f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.5f;
    private Animator anim;
    private Rigidbody2D rb;
    private float lastDashTime;
    private float dashTimer;
    private bool isDashing;
    private Vector2 dashDir;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        lastDashTime = Time.time;

        player = NKY_GameManager.instance.player.transform;
    }
    private void Update()
    {
        if (player == null)
        {
            anim.SetFloat("Speed", 0f);
            return;
        }
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            anim.SetFloat("Speed", dashSpeed);
            if (dashTimer <= 0f)
            {
                isDashing = false;
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }
        if (Time.time >= lastDashTime + dashInterval)
        {
            StartDash();
            return;
        }
        float distance = Vector2.Distance(transform.position, player.position);
        float currentSpeed = 0f;
        if (distance <= followRange && distance > keepDistance)
        {
            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = dir * moveSpeed; // ← 핵심 변경
            currentSpeed = moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // ← 범위 밖이면 정지
        }
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
        anim.SetFloat("Speed", currentSpeed);
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = dashDir * dashSpeed; // ← 대시는 FixedUpdate에서
        }
    }
    private void StartDash()
    {
        lastDashTime = Time.time;
        isDashing = true;
        dashTimer = dashDuration;
        dashDir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        anim.SetTrigger("Attack");
    }
}