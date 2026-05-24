using System.Collections;
using UnityEngine;

public class JHY_BossMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator ani;
    private SpriteRenderer sr;
    private JHY_Attack bossAttack; // ◀ 공격 스크립트 참조 추가

    [SerializeField] private float speed = 2;
    private Transform player;
    private Health playerHealth; // ◀ 인스펙터에서 안 넣어도 자동으로 찾도록 수정

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

    void Awake()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bossAttack = GetComponent<JHY_Attack>(); // ◀ 컴포넌트 가져오기

        timer = stunTime;
        dashTimer = dashCooldown;
    }

    private void Start()
    {
        // ◀ 공격 스크립트와 싱크를 맞추기 위해 자체적인 Start 코루틴 대기는 삭제합니다.
        // ◀ 대신 시작할 때 확실하게 매니저에서 플레이어를 세팅합니다.
        SetPlayerTarget();
    }

    public void SetPlayerTarget()
    {
        if (NKY_GameManager.instance != null && NKY_GameManager.instance.player != null)
        {
            player = NKY_GameManager.instance.player.transform;
            playerHealth = player.GetComponent<Health>();
        }
    }

    void Update()
    {
        // ★ [핵심] 공격 스크립트의 canAct 상태를 그대로 따라갑니다.
        if (bossAttack == null || !bossAttack.canAct) return;

        // 만약 어떤 이유로든 플레이어를 못 찾았다면 다시 시도
        if (player == null)
        {
            SetPlayerTarget();
            return;
        }

        if (playerHealth != null && playerHealth.IsDestroyed)
        {
            StopMoving();
            StopAllCoroutines();
            isDashing = false;
            isStunned = false;
            return;
        }

        if (isStunned || isDashing || isSkillLocked)
        {
            StopMoving();
            return;
        }

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

        NKY_SoundManager.Instance.PlaySFX("Rush");
        Vector2 dashDirection = (player.position - transform.position).normalized;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            if (isSkillLocked) break;

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
        NKY_SoundManager.Instance.PlaySFX("Stun");
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
