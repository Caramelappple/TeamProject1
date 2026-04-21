using UnityEngine;
using System.Collections;

public class JHY_Attack : MonoBehaviour
{
    private Animator ani;
    private SpriteRenderer sr;
    private JHY_BossMove bossMove;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float skillRange = 4f;
    [SerializeField] private float skillCoolTime = 5f;

    [Header("Skill Details")]
    [SerializeField] private int projectileCount = 5;
    [SerializeField] private float spreadAngle = 60f;
    [SerializeField] private float detectRangeX = 1.0f;
    [SerializeField] private int shockwaveProjectileCount = 12;
    [SerializeField] private float shockwaveStartAngleOffset = 0f;
    [SerializeField] private float jumpAttackCoolTime = 5f;
    private float lastJumpAttackTime;


    private float lastAttackTime;
    private float lastSkillTime;
    private bool isSkillUsing = false;
    private Vector3 firePointBaseLocalPos;

    private Vector3 bossBaseScale;

    void Awake()
    {
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bossMove = GetComponent<JHY_BossMove>();

        bossBaseScale = transform.localScale;
        firePointBaseLocalPos = firePoint.localPosition;
        firePointBaseLocalPos.x = -firePointBaseLocalPos.x;
    }

    void Update()
    {
        if (player == null) return;

        FacePlayer();

        if (isSkillUsing || (bossMove != null && bossMove.isMoving)) return;

        float distance = Vector2.Distance(transform.position, player.position);
        float diffX = Mathf.Abs(player.position.x - transform.position.x);

        if (diffX <= detectRangeX && Time.time >= lastJumpAttackTime + jumpAttackCoolTime)
        {
            lastJumpAttackTime = Time.time;
            StartCoroutine(JumpAttackWrapper());
            return;
        }

        if (distance <= skillRange && Time.time >= lastSkillTime + skillCoolTime)
        {
            if (IsPlayerInHorizontalRange() && IsPlayerOutsideFirePoint())
            {
                UseSkill();
                return;
            }
        }

        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            ani.SetTrigger("attack");
            lastAttackTime = Time.time;
        }
    }

    void FacePlayer()
    {
        bool faceLeft = player.position.x < transform.position.x;
        float dir = faceLeft ? -1f : 1f;

        transform.localScale = new Vector3(
            Mathf.Abs(bossBaseScale.x) * dir,
            bossBaseScale.y,
            bossBaseScale.z
        );

        if (sr != null)
        {
            sr.flipX = false;
        }

        if (firePoint != null)
        {
            firePoint.localPosition = firePointBaseLocalPos;
        }
    }

    void UseSkill()
    {
        isSkillUsing = true;
        lastSkillTime = Time.time;
        bossMove?.StopMoving();
        ani.SetTrigger("Skill");
        Invoke(nameof(EndSkill), 1.5f);
    }

    public void EndSkill() => isSkillUsing = false;

    public void FireProjectiles()
    {
        if (player == null || firePoint == null || projectilePrefab == null) return;

        Vector2 dirToPlayer = (player.position - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        if (projectileCount <= 1)
        {
            Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, baseAngle));
            return;
        }

        float startAngle = baseAngle - (spreadAngle / 2f);
        float angleStep = spreadAngle / (projectileCount - 1);

        for (int i = 0; i < projectileCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, currentAngle));
        }
    }

    bool IsPlayerInHorizontalRange()
    {
        Vector2 lookDir = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        Vector2 targetDir = (player.position - transform.position).normalized;
        float angle = Vector2.Angle(lookDir, targetDir);

        return angle < 70f;
    }

    bool IsPlayerOutsideFirePoint()
    {
        if (player == null || firePoint == null) return false;

        Vector2 lookDir = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        float playerForwardDistance = Vector2.Dot(player.position - transform.position, lookDir);
        float firePointForwardDistance = Vector2.Dot(firePoint.position - transform.position, lookDir);

        return playerForwardDistance >= firePointForwardDistance;
    }

    IEnumerator JumpAttackWrapper()
    {
        isSkillUsing = true;
        bossMove?.StopMoving();
        yield return StartCoroutine(JumpAttackRoutine());
        yield return new WaitForSeconds(2.0f);
        isSkillUsing = false;
    }

    IEnumerator JumpAttackRoutine()
    {
        ani.SetTrigger("Jump");
        yield return new WaitForSeconds(2.1f);
        SpawnShockwave();
    }

    public void SpawnShockwave()
    {
        Debug.Log("쾅!");
        if (projectilePrefab == null) return;

        Vector3 spawnPosition = transform.position;
        float angleStep = 360f / shockwaveProjectileCount;

        for (int i = 0; i < shockwaveProjectileCount; i++)
        {
            float currentAngle = shockwaveStartAngleOffset + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(0f, 0f, currentAngle);
            Instantiate(projectilePrefab, spawnPosition, rotation);
        }
    }
}