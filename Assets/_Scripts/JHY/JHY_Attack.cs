using UnityEngine;
using System.Collections;
public class JHY_Attack : MonoBehaviour
{
    /*[Header("AudioClips")]
    [SerializeField] private AudioClip spiderWebClip;
    [SerializeField] private AudioClip phase2Clip;
    [SerializeField] private AudioClip meleeClip;
    [SerializeField] private AudioClip spearClip;
    [SerializeField] private AudioClip summonClip;
    [SerializeField] private AudioClip monsterDeathClip;
    [SerializeField] private AudioClip jumpAttackClip;
    [SerializeField] private AudioClip fireProjectilesClip;
    [SerializeField] private AudioClip bossDeathClip;*/
    
    //호연아 제발 나눠서 잘 해라 뭐가 뭔지 하나도 모르겠다.
    //주석이라도 달아주던가 해야지 진짜 오디오 넣을려는데 모르겠다.
    //내일 보면 주석 달아두던가 너가 오디오 넣어라.
    //-이시온-
    
    
    private Animator _ani;
    private SpriteRenderer _sr;
    private JHY_BossMove _bossMove;
    
    [Header("References")]
    [SerializeField]private Transform player;
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
    [SerializeField] private float shockwaveStartAngleOffset;
    [SerializeField] private float skillAngle = 60f;

    [Header("Spider Web")]
    [SerializeField] private GameObject spiderWebPrefab;
    [SerializeField] private float spiderWebSpawnTimer = 15f;
    [SerializeField] private float spiderWebDuration = 6f;


    [Header("RainSpear")]
    [SerializeField] private SpearSpawner spearSpawner;
    [SerializeField] private float spearRainCoolTime = 20f;
    private float _lastSpearRainTime;

    [Header("Damage")]
    [SerializeField] private int meleeDamage = 10;
    [SerializeField] private Health playerHealth;

    [Header("Summon Skill")]
    [SerializeField] private float summonCoolTime = 40f;
    [SerializeField] private JHY_MobSummoner mobSummoner;
    private float _lastSummonTime;
    private bool _isSummoning;

    [Header("Phase")]
    [SerializeField] private Health bossHealth;
    [SerializeField] private float phase2Threshold = 0.5f;
    private bool _isPhase2;
    [SerializeField] private GameObject phase2EffectPrefab;
    [SerializeField] private float phase2EffectLifeTime = 2f;
    [SerializeField] private GameObject phase2Aura;
    private bool _isPhaseChanging;
    [SerializeField] private float phaseChangeDuration = 1.5f;
    private GameObject _spawnedAura;

    private float _lastAttackTime;
    private float _lastSkillTime;
    private float _lastJumpAttackTime;
    private bool _isSkillUsing;
    private Vector3 _firePointBaseLocalPos;
    private Vector3 _bossBaseScale;
    [SerializeField] private JHY_WarningZone warningZone;

    void Awake()
    {
        _ani = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _bossMove = GetComponent<JHY_BossMove>();

        _bossBaseScale = transform.localScale;
    }
    void Start()
    {
        player = NKY_GameManager.instance.player.transform;

        StartCoroutine(SpiderWebRoutine());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        CancelInvoke();
    }
    IEnumerator SpiderWebRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spiderWebSpawnTimer);
            if (!enabled) yield break;
            if (playerHealth != null && playerHealth.IsDestroyed) yield break;
            if (_isSummoning) continue;
            if (_isSkillUsing) continue;
            if (spiderWebPrefab == null) continue;

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
        if (player == null) return;
        if (playerHealth != null && playerHealth.IsDestroyed) return;
        CheckPhase2();
        if (_isPhaseChanging) return;
        FacePlayer();

        if (_isSummoning)
        {
            _bossMove?.StopMoving();

            if (mobSummoner != null && !mobSummoner.HasAliveSummons())
            {
                EndSummonState();
            }

            return;
        }
        if (_isSkillUsing || (_bossMove != null && (_bossMove.isMoving || _bossMove.isStunned))) return;

        if (Time.time >= _lastSpearRainTime + spearRainCoolTime)
        {
            UseSpearRain();
            return;
        }
        if (Time.time >= _lastSummonTime + summonCoolTime)
        {
            UseSummonSkill();
            return;
        }


        float skillDistance = Vector2.Distance(firePoint.position, player.position);
        float attackDistance = Vector2.Distance(transform.position, player.position);
        float diffX = Mathf.Abs(player.position.x - transform.position.x);

        if (diffX <= detectRangeX && Time.time >= _lastJumpAttackTime + jumpAttackCoolTime)
        {
            _lastJumpAttackTime = Time.time;
            StartCoroutine(JumpAttackWrapper());
            return;
        }

        if (skillDistance <= skillRange && Time.time >= _lastSkillTime + skillCoolTime)
        {
            if (IsPlayerOutsideFirePoint() && IsPlayerInSkillAngle())
            {
                UseSkill();
                return;
            }
        }

        if (attackDistance <= attackRange && Time.time >= _lastAttackTime + attackCooldown)
        {
            _ani.SetTrigger("attack");
            _lastAttackTime = Time.time;
        }
    }
    private void CheckPhase2()
    {
       
    
        if (_isPhase2 || _isPhaseChanging) return;
        if (bossHealth == null) return;

        float hpPercent = (float)bossHealth.Value / bossHealth.MaxValue;
        if (hpPercent <= phase2Threshold)
        {
            StartCoroutine(EnterPhase2Routine());
        }
    
    }
    private IEnumerator EnterPhase2Routine()
    {
        _isPhaseChanging = true;
        _isSkillUsing = true;

        _bossMove?.StopMoving();
        _bossMove?.SetSkillLock(true);

        yield return new WaitForSeconds(phaseChangeDuration);

        EnterPhase2();

        _isPhaseChanging = false;
        _isSkillUsing = false;
        _bossMove?.SetSkillLock(false);
    }
    private void EnterPhase2()
    {
        _isPhase2 = true;
        if (phase2EffectPrefab != null)
        {
            GameObject effect = Instantiate(phase2EffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, phase2EffectLifeTime);
        }


        {
            if (phase2Aura != null && _spawnedAura == null)
            {
                _spawnedAura = Instantiate(phase2Aura, transform.position, Quaternion.identity, transform);
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
        if (_spawnedAura != null)
        {
            Destroy(_spawnedAura);
            _spawnedAura = null;
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
        _lastSummonTime = Time.time;
        _isSummoning = true;
        _isSkillUsing = true;

        _bossMove?.StopMoving();
        _bossMove?.SetSkillLock(true);

        _ani.ResetTrigger("attack");
        _ani.ResetTrigger("Jump");
        _ani.ResetTrigger("Skill");
        
        if (mobSummoner != null)
        {
            mobSummoner.SummonMobs();
        }
    }

    private void EndSummonState()
    {
        _isSummoning = false;
        _isSkillUsing = false;
        _bossMove?.SetSkillLock(false);
    }



    private void UseSpearRain()
    {
        _lastSpearRainTime = Time.time;
        _isSkillUsing = true;
        _bossMove?.SetSkillLock(true);

        _ani.SetTrigger("Skill");

        if (spearSpawner != null)
        {
            spearSpawner.SpawnSpears();
        }

        Invoke(nameof(EndSpearRain), 1.5f);
    }

    private void EndSpearRain()
    {
        _bossMove?.SetSkillLock(false);
        _isSkillUsing = false;
    }

    void FacePlayer()
    {
        if (_bossMove != null && _bossMove.isStunned) return;
        bool faceLeft = player.position.x < transform.position.x;
        float dir = faceLeft ? -1f : 1f;

        transform.localScale = new Vector3(
            Mathf.Abs(_bossBaseScale.x) * dir,
            _bossBaseScale.y,
            _bossBaseScale.z
        );

        if (_sr != null)
        {
            _sr.flipX = false;
        }


    }

    void UseSkill()
    {
        _isSkillUsing = true;
        _lastSkillTime = Time.time;
        _bossMove?.StopMoving();


        _ani.ResetTrigger("attack");
        _ani.ResetTrigger("Jump");
        _ani.SetTrigger("Skill");
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
        _isSkillUsing = false;
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
        _isSkillUsing = true;
        _bossMove?.SetSkillLock(true);

        yield return StartCoroutine(JumpAttackRoutine());
        yield return new WaitForSeconds(2.0f);

        _bossMove?.SetSkillLock(false);
        _isSkillUsing = false;
    }


    IEnumerator JumpAttackRoutine()
    {
        _ani.SetTrigger("Jump");

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