using System.Collections;
using UnityEngine;

public class JHY_BossMove : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    private Rigidbody2D rb;
    private Animator ani;
    [SerializeField] private float speed = 2;
    [SerializeField] private Transform player;
    private SpriteRenderer sr;
    [SerializeField] private float chaseRange = 7f;
    [SerializeField] private float stopRange = 4f;
    private bool isArrived = false;
    public bool isMoving;

    [SerializeField] private float stunTime = 15f;
    [SerializeField] private float stunDuration = 7f;

    public bool isStunned;
    private float timer;

    [Header("Dash")]
    [SerializeField] private float dashRange = 10f;
    [SerializeField] private float dashCooldown = 8f;
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.7f;
    private float dashTimer;
    private bool isDashing;

    private bool isSkillLocked;

    private bool canAct = false;
    [SerializeField] private float startDelay = 5f;
    void Awake()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        timer = stunTime;
        dashTimer = dashCooldown;

        //player = NKY_GameManager.instance.player.transform;
    }

    private IEnumerator Start()
    {
        canAct = false;
        yield return new WaitForSeconds(startDelay);
        canAct = true;
    }
    void Update()
    {
        if (!canAct) return;
        if (playerHealth != null && playerHealth.IsDestroyed)
        {
            StopMoving();
            StopAllCoroutines(); // 진행 중인 스턴/돌진 코루틴 중단
            isDashing = false;
            isStunned = false;
            return;
        }
        if (isStunned || isDashing || isSkillLocked)
        {
            StopMoving();
            return;
        }

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

        //보스가 빠르게 돌진 소리
        Vector2 dashDirection = (player.position - transform.position).normalized;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            if (isSkillLocked)
            {
                break;
            }

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
        //보스가 스턴 소리
        yield return new WaitForSeconds(stunDuration);
        //보스가 정신을 차리고 다시 일어나는 짧은 효과음 재생
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
        //보스 움직임소리
    }

    public void StopMoving()
    {
        isMoving = false;
        ani.SetFloat("Speed", 0f);
    }

    public void SetSkillLock(bool value)
    {
        isSkillLocked = value;

        if (value)
        {
            isDashing = false;
            StopMoving();
        }
    }
}
