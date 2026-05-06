using System.Collections;
using UnityEngine;

public class JHY_BossMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator ani;
    //move
    [SerializeField] private float speed;
    [SerializeField] private Transform player;
    private SpriteRenderer sr;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float stopRange = 2f;
    private bool isArrived = false;
    public bool isMoving;

    [SerializeField] private float stunTime = 20f;
    [SerializeField] private float stunDuration = 7f;

    public bool isStunned;
    private float timer;

    [Header("Dash")]
    [SerializeField] private float dashRange = 10f;        // 이 거리 안에 있으면 돌진 가능
    [SerializeField] private float dashCooldown = 10f;    // 10초마다
    [SerializeField] private float dashSpeed = 12f;       // 돌진 속도
    [SerializeField] private float dashDuration = 0.7f;   // 돌진 시간
    private float dashTimer;
    private bool isDashing;
    void Awake()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        timer = stunTime;
        dashTimer = dashCooldown;
    }

    void Update()
    {
        if (isStunned||isDashing) return;
        if (player == null) return;
        if (isArrived)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StartCoroutine(Stun());
                return;
            }
        }

        
        LookAtPlayer();

        
        if (!isArrived)
        {
            Vector2 targetPos = new Vector2(3.56f, -0.83f);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            float distance = Vector2.Distance(transform.position, targetPos);
            isMoving = true;
            ani.SetFloat("Speed", speed);

            if (distance < 0.01f)
            {
                isArrived = true;
                isMoving = false;
                ani.SetFloat("Speed", 0);
            }
        }
        else
        {
            float distance2 = Vector2.Distance(transform.position, player.position);

            dashTimer -= Time.deltaTime;

            // 플레이어가 dashRange 안에 있고, 쿨타임이 끝났으면 돌진
            if (distance2 <= dashRange && dashTimer <= 0f)
            {
                StartCoroutine(DashToPlayer());
                return;
            }
            if (distance2 > chaseRange)
            {
                MoveTowardsPlayer();
            }
            else if (distance2 <= stopRange)
            {
                StopMoving();
            }
            else
            {
                isMoving = false;
                ani.SetFloat("Speed", 0);
            }
        }
    }
    IEnumerator DashToPlayer()
    {
        isDashing = true;
        isMoving = true;
        dashTimer = dashCooldown;

        Vector2 dashDirection = (player.position - transform.position).normalized;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            transform.position += (Vector3)(dashDirection * dashSpeed * Time.deltaTime);
           

            ani.SetFloat("Speed", dashSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        isMoving = false;
        ani.SetFloat("Speed", 0f);
    }
    IEnumerator Stun()
    {
        isStunned = true;
        isMoving = false;
        ani.SetFloat("Speed", 0f);
        ani.SetTrigger("Stop");

        yield return new WaitForSeconds(stunDuration);

        timer = stunTime;
        isStunned = false;
    }

    void LookAtPlayer()
    {
        if (player.position.x > transform.position.x)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 targetPos = new Vector2(player.position.x, player.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        isMoving = true;
        ani.SetFloat("Speed", speed);
    }

    public void StopMoving()
    {
        isMoving = false;
        ani.SetFloat("Speed", 0f);
    }
}