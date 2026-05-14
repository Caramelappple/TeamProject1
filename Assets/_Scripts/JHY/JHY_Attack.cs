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
    [SerializeField] private GameObject projectilePrefab2;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float skillRange = 4f;
    [SerializeField] private float skillCoolTime = 5f;
    [SerializeField] private float jumpAttackCoolTime = 5f;

    [Header("Skill Details")]
    [SerializeField] private int projectileCount = 5;
    [SerializeField] private float spreadAngle = 60f;
    [SerializeField] private float detectRangeX = 1.0f;
    [SerializeField] private int shockwaveProjectileCount = 12;
    [SerializeField] private float shockwaveStartAngleOffset = 0f;
    [SerializeField] private float skillAngle = 60f;

    [Header("Spider Web")]
    [SerializeField] private GameObject spiderWebPrefab;
    [SerializeField] private float spiderWebSpawnTimer = 15f;
    [SerializeField] private float spiderWebDuration = 6f;


    [Header("RainSpear")]
    [SerializeField] private SpearSpawner spearSpawner;
    [SerializeField] private float spearRainCoolTime = 20f;
    private float lastSpearRainTime;



    private float lastAttackTime;
    private float lastSkillTime;
    private float lastJumpAttackTime;
    private bool isSkillUsing = false;
    private Vector3 firePointBaseLocalPos;
    private Vector3 bossBaseScale;
    [SerializeField] private JHY_WarningZone warningZone;

    void Awake()
    {
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bossMove = GetComponent<JHY_BossMove>();

        bossBaseScale = transform.localScale;


    }
    void Start()
    {
        StartCoroutine(SpiderWebRoutine());
    }

    IEnumerator SpiderWebRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spiderWebSpawnTimer);

            if (spiderWebPrefab != null)
            {
                GameObject web = Instantiate(
                    spiderWebPrefab,
                    transform.position,
                    Quaternion.identity
                );

                Destroy(web, spiderWebDuration);
            }
        }
    }
    void Update()
    {
        if (player == null) return;

        FacePlayer();

        if (isSkillUsing || (bossMove != null && (bossMove.isMoving || bossMove.isStunned))) return;

        if (Time.time >= lastSpearRainTime + spearRainCoolTime)
        {
            UseSpearRain();
            return;
        }

        float skilldistance = Vector2.Distance(firePoint.position, player.position);
        float attackdistance = Vector2.Distance(transform.position, player.position);
        float diffX = Mathf.Abs(player.position.x - transform.position.x);

        if (diffX <= detectRangeX && Time.time >= lastJumpAttackTime + jumpAttackCoolTime)
        {
            lastJumpAttackTime = Time.time;
            StartCoroutine(JumpAttackWrapper());
            return;
        }

        if (skilldistance <= skillRange && Time.time >= lastSkillTime + skillCoolTime)
        {
            if (IsPlayerOutsideFirePoint() && IsPlayerInSkillAngle())
            {
                UseSkill();
                return;
            }
        }

        if (attackdistance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            ani.SetTrigger("attack");
            lastAttackTime = Time.time;
        }
    }
    private void UseSpearRain()
    {
        lastSpearRainTime = Time.time;
        isSkillUsing = true;
        bossMove?.SetSkillLock(true);

        ani.SetTrigger("Skill");

        if (spearSpawner != null)
        {
            spearSpawner.SpawnSpears();
        }

        Invoke(nameof(EndSpearRain), 1.5f);
    }

    private void EndSpearRain()
    {
        bossMove?.SetSkillLock(false);
        isSkillUsing = false;
    }

    void FacePlayer()
    {
        if (bossMove != null && bossMove.isStunned) return;
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


    }

    void UseSkill()
    {
        isSkillUsing = true;
        lastSkillTime = Time.time;
        bossMove?.StopMoving();


        ani.ResetTrigger("attack");
        ani.ResetTrigger("Jump");
        ani.SetTrigger("Skill");
        Invoke(nameof(EndSkill), 1.5f);
    }
    bool IsPlayerInSkillAngle()
    {
        Vector2 lookDir = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        Vector2 toPlayer = (player.position - firePoint.position).normalized;

        float angle = Vector2.Angle(lookDir, toPlayer);
        return angle <= skillAngle * 0.5f;
    }


    public void EndSkill()
    {
        isSkillUsing = false;
    }

    public void FireProjectiles()
    {
        Debug.Log("FireProjectiles called");

        if (player == null || firePoint == null || projectilePrefab == null) return;

        Vector2 dirToPlayer = (player.position - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        if (projectileCount <= 1)
        {
            Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0f, 0f, baseAngle));
            return;
        }

        float startAngle = baseAngle - (spreadAngle / 2f);
        float angleStep = spreadAngle / (projectileCount - 1);

        for (int i = 0; i < projectileCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0f, 0f, currentAngle));
        }
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
        bossMove?.SetSkillLock(true);

        yield return StartCoroutine(JumpAttackRoutine());
        yield return new WaitForSeconds(2.0f);

        bossMove?.SetSkillLock(false);
        isSkillUsing = false;
    }


    IEnumerator JumpAttackRoutine()
    {
        ani.SetTrigger("Jump");

        yield return new WaitForSeconds(0.6f);

        if (warningZone != null)
        {
            warningZone.Warning();
        }

        yield return new WaitForSeconds(1.5f);
    }


    public void SpawnShockwave_First()
    {
        SpawnShockwaveWithOffset(0f);
    }

    public void SpawnShockwave_Second()
    {
        if (shockwaveProjectileCount <= 0) return;

        float angleStep = 360f / shockwaveProjectileCount;
        SpawnShockwaveWithOffset(angleStep * 0.5f);
    }

    private void SpawnShockwaveWithOffset(float extraOffset)
    {
        Debug.Log("Shockwave!");

        if (projectilePrefab2 == null) return;
        if (shockwaveProjectileCount <= 0) return;

        Vector3 spawnPosition = transform.position;
        float angleStep = 360f / shockwaveProjectileCount;

        for (int i = 0; i < shockwaveProjectileCount; i++)
        {
            float currentAngle = shockwaveStartAngleOffset + (angleStep * i) + extraOffset;
            Quaternion rotation = Quaternion.Euler(0f, 0f, currentAngle);
            Instantiate(projectilePrefab2, spawnPosition, rotation);
        }
    }

}