using UnityEngine;
using System.Collections;
public class JHY_Attack : MonoBehaviour
{
    private Animator ani;
    private SpriteRenderer sr;
    private JHY_BossMove bossMove;

    [Header("References")]
    [SerializeField]private Transform player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject projectilePrefab2;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 4f;
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private float skillRange = 15f;
    [SerializeField] private float skillCoolTime = 5f;
    [SerializeField] private float jumpAttackCoolTime = 7f;

    [Header("Skill Details")]
    [SerializeField] private int projectileCount = 5;
    [SerializeField] private float spreadAngle = 60f;
    [SerializeField] private float detectRangeX = 1.0f;
    [SerializeField] private int shockwaveProjectileCount = 12;
    [SerializeField] private float shockwaveStartAngleOffset = 0f;
    [SerializeField] private float skillAngle = 100f;

    [Header("Spider Web")]
    [SerializeField] private GameObject spiderWebPrefab;
    [SerializeField] private float spiderWebSpawnTimer = 13f;
    [SerializeField] private float spiderWebDuration = 6f;


    [Header("RainSpear")]
    [SerializeField] private SpearSpawner spearSpawner;
    [SerializeField] private float spearRainCoolTime = 20f;
    private float lastSpearRainTime;

    [Header("Damage")]
    [SerializeField] private int meleeDamage = 5;
    [SerializeField] private Health playerHealth;

    [Header("Summon Skill")]
    [SerializeField] private float summonCoolTime = 40f;
    [SerializeField] private JHY_MobSummoner mobSummoner;
    private float lastSummonTime;
    private bool isSummoning;

    [Header("Phase")]
    [SerializeField] private Health bossHealth;
    [SerializeField] private float phase2Threshold = 0.5f;
    private bool isPhase2;
    [SerializeField] private GameObject phase2EffectPrefab;
    [SerializeField] private float phase2EffectLifeTime = 1.5f;
    [SerializeField] private GameObject phase2Aura;
    private bool isPhaseChanging;
    [SerializeField] private float phaseChangeDuration = 1.5f;
    private GameObject spawnedAura;

    private float lastAttackTime;
    private float lastSkillTime;
    private float lastJumpAttackTime;
    private bool isSkillUsing = false;
    private Vector3 firePointBaseLocalPos;
    private Vector3 bossBaseScale;
    [SerializeField] private JHY_WarningZone warningZone;

    public bool canAct = false;
    [SerializeField] private float startDelay = 5f;
    void Awake()
    {
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bossMove = GetComponent<JHY_BossMove>();

        bossBaseScale = transform.localScale;
    }
    private IEnumerator Start()
    {
        canAct = false;

        if (NKY_GameManager.instance != null && NKY_GameManager.instance.player != null)
        {
            player = NKY_GameManager.instance.player.transform;
            playerHealth = player.GetComponent<Health>();

            if (bossMove != null) bossMove.SetPlayerTarget();
        }

        StartCoroutine(SpiderWebRoutine());

        yield return new WaitForSeconds(startDelay);

        float sceneTime = Time.time;

      
        lastAttackTime = sceneTime;
        lastSkillTime = sceneTime;
        lastJumpAttackTime = sceneTime;
        lastSpearRainTime = sceneTime;
        lastSummonTime = sceneTime; 

        canAct = true;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        CancelInvoke();
    }
    IEnumerator SpiderWebRoutine()
    {
       
        while (!canAct)
        {
            yield return null; 
        }
        while (true)
        {
            yield return new WaitForSeconds(spiderWebSpawnTimer);
            if (!enabled) yield break;
            if (!canAct) continue;
            if (playerHealth != null && playerHealth.IsDestroyed) yield break;
            if (isSummoning) continue;
            if (isSkillUsing) continue;
            if (spiderWebPrefab == null) continue;

            NKY_SoundManager.Instance.PlaySFX("SpiderWeb");
            GameObject web = Instantiate(
                spiderWebPrefab,
                transform.position,
                Quaternion.identity
            );

            Destroy(web, spiderWebDuration);
        }
    }

    void Update()
    {
        if (!canAct) return;
        if (player == null) return;
        if (playerHealth != null && playerHealth.IsDestroyed) return;
        CheckPhase2();
        if (isPhaseChanging) return;
        FacePlayer();

        if (isSummoning)
        {
            bossMove?.StopMoving();

            if (mobSummoner != null && !mobSummoner.HasAliveSummons())
            {
                EndSummonState();
            }

            return;
        }
        if (isSkillUsing || (bossMove != null && (bossMove.isMoving || bossMove.isStunned))) return;

        if (Time.time >= lastSpearRainTime + spearRainCoolTime)
        {
            UseSpearRain();
            return;
        }
        if (Time.time >= lastSummonTime + summonCoolTime)
        {
            UseSummonSkill();
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
            // r기본 근접공격 사운드
            Invoke(nameof(PlayMeleeAttackSound), 0.25f);
        }
    }
    public void PlayMeleeAttackSound()
    {
        if (NKY_SoundManager.Instance != null)
        {
            NKY_SoundManager.Instance.PlaySFX("Melee");
        }
    }
    private void CheckPhase2()
    {
       
    
        if (isPhase2 || isPhaseChanging) return;
        if (bossHealth == null) return;

        float hpPercent = (float)bossHealth.Value / bossHealth.MaxValue;
        if (hpPercent <= phase2Threshold)
        {
            StartCoroutine(EnterPhase2Routine());
        }
    
}
    private IEnumerator EnterPhase2Routine()
    {
        isPhaseChanging = true;
        isSkillUsing = true;

        bossMove?.StopMoving();
        bossMove?.SetSkillLock(true);

        yield return new WaitForSeconds(phaseChangeDuration);

        EnterPhase2();

        isPhaseChanging = false;
        isSkillUsing = false;
        bossMove?.SetSkillLock(false);
    }
    private void EnterPhase2()
    {
        isPhase2 = true;
        //2페이즈 변신 효과음 재생
        NKY_SoundManager.Instance.PlaySFX("Phase2");
        if (phase2EffectPrefab != null)
        {
            GameObject effect = Instantiate(phase2EffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, phase2EffectLifeTime);
        }


        {
            if (phase2Aura != null && spawnedAura == null)
            {
                spawnedAura = Instantiate(phase2Aura, transform.position, Quaternion.identity, transform);
            }

            attackCooldown = 1.0f;
            skillCoolTime = 3.0f;
            jumpAttackCoolTime = 3.0f;
            spearRainCoolTime = 12.0f;
            summonCoolTime = 25.0f;
            projectileCount = 7;
            shockwaveProjectileCount = 16;
            spiderWebSpawnTimer = 8.0f;

            Debug.Log("2페이즈 진입");
        }
    }
    public void ClearAura()
    {
        if (spawnedAura != null)
        {
            Destroy(spawnedAura);
            spawnedAura = null;
        }
    }
    public void DealMeleeDamage()
    {
        if (playerHealth == null || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > attackRange) return;

        DamageData data = DamageData.Create(null, meleeDamage);
        playerHealth.GetDamage(data);
    }
    private void UseSummonSkill()
    {
        lastSummonTime = Time.time;
        isSummoning = true;
        isSkillUsing = true;

        bossMove?.StopMoving();
        bossMove?.SetSkillLock(true);

        ani.ResetTrigger("attack");
        ani.ResetTrigger("Jump");
        ani.ResetTrigger("Skill");
        
        if (mobSummoner != null)
        {
            mobSummoner.SummonMobs();
            NKY_SoundManager.Instance.PlaySFX("MobSpawn");
        }
    }

    private void EndSummonState()
    {
        isSummoning = false;
        isSkillUsing = false;
        bossMove?.SetSkillLock(false);
    }



    private void UseSpearRain()
    {
        lastSpearRainTime = Time.time;
        isSkillUsing = true;
        bossMove?.SetSkillLock(true);

        ani.SetTrigger("Skill");
        //여기에 창 비 시전 마법 효과음/ 보스 포효 소리 재생
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
        //스킬 발사하는 소리
        Invoke(nameof(PlaySkillSound), 0.65f);
        Invoke(nameof(EndSkill), 1.5f);
    }
    void PlaySkillSound()
    {
        NKY_SoundManager.Instance.PlaySFX("XFire");
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
        //점프 올라가는 소리
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
        //점프 착지 충격파 소리
        NKY_SoundManager.Instance.PlaySFX("Jump");
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