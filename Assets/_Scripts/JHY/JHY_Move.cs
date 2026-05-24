using System.Collections;
using UnityEngine;

public class JHY_BossMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator ani;
    private SpriteRenderer sr;
    private JHY_Attack bossAttack;

    [SerializeField] private float speed = 2;
    private Transform player;
    private Health playerHealth;

    [SerializeField] private float chaseRange = 7f;
    [SerializeField] private float stopRange = 4f;
    public bool isArrived = false;
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

    [Header("intro")]
    [SerializeField] private NKY_BossIntro bossIntro;
    private float dashTimer;
    private bool isDashing;
    private bool isSkillLocked;

    void Awake()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bossAttack = GetComponent<JHY_Attack>();

        timer = stunTime;
        dashTimer = dashCooldown;
    }

    private void Start()
    {
        StartCoroutine(PlayBoss());
    }

    private IEnumerator PlayBoss()
    {
        yield return StartCoroutine(bossIntro.PlayIntro());
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
        if (bossAttack == null || !bossAttack.canAct) return;

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

        if (!isArrived)
        {
            Vector2 targetPos = new Vector2(3.56f, -0.83f);
            Vector2 moveDir = (targetPos - (Vector2)transform.position).normalized;
            LookAtDirection(moveDir);

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
            LookAtPlayer();

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


    void LookAtDirection(Vector2 dir)
    {
        if (sr == null || dir.x == 0) return;

        
        if (dir.x > 0)
            sr.flipX = false;
        else if (dir.x < 0)
            sr.flipX = true;
    }

  
    void LookAtPlayer()
    {
        if (player == null || sr == null) return;

       
        if (player.position.x > transform.position.x)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
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
