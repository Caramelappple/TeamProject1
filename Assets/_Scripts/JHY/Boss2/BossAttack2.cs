using UnityEngine;
using System.Collections;
public class BossAttack2 : MonoBehaviour
{
    public Transform player;
    public float attackRange = 3.3f;
    public float attackCooldown = 3f;
    public int damage = 5;
    public float comboCooldown = 8f;
    public float comboRange = 3.3f;
    private float lastComboTime;
   private Health playerHealth;
    private Animator anim;
    private Health health;
    private BossMove2 bossFollow;
    private float lastAttackTime;
    private bool isDead;
    private Rigidbody2D rb;
    [Header("Combo Warning")]
    public GameObject comboWarningPrefab;
    public float comboWarningTime = 1f;

    public bool IsPreparingCombo => isPreparingCombo;
    private bool isPreparingCombo;
    private GameObject currentComboWarning;
    public bool IsAttacking { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        bossFollow = GetComponent<BossMove2>();
        lastAttackTime = -attackCooldown;

        player = NKY_GameManager.instance.player.transform;

        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }
    }

    private void Update()
    {
        
        if (playerHealth != null && playerHealth.IsDestroyed) return;
        SyncAttackState();

        if (player == null || isDead) return;
        if (bossFollow != null && bossFollow.IsDashingNow) return;
        if (IsAttacking) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= comboRange && Time.time >= lastComboTime + comboCooldown && !IsAttacking)
        {
            lastComboTime = Time.time;
            anim.SetTrigger("ComboAttack");
            return;
        }

        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            RemoveComboWarning();
            lastAttackTime = Time.time;
            anim.SetTrigger("Attack");
        }
    }

    public void SpawnComboWarning()
    {
        RemoveComboWarning();

        if (comboWarningPrefab == null) return;

        Vector3 spawnPos = transform.position;
        currentComboWarning = Instantiate(comboWarningPrefab, spawnPos, Quaternion.identity);
    }

    public void RemoveComboWarning()
    {
        if (currentComboWarning != null)
        {
            Destroy(currentComboWarning);
            currentComboWarning = null;
        }
    }

    public void OnAttackStart()
    {

        if (bossFollow != null && bossFollow.IsDashingNow)
        {
            IsAttacking = false;
            health?.SetDamageable(true);
            return;
        }

        IsAttacking = true;
        health?.SetDamageable(false);
    }
    public void OnAttackEnd()
    {
        IsAttacking = false;
        health?.SetDamageable(true);
    }

    private void SyncAttackState()
    {
        if (anim == null) return;
      
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        
            bool inAttackState = stateInfo.IsTag("Attack");

        if (!inAttackState && IsAttacking)
        {
            IsAttacking = false;
            health?.SetDamageable(true);
            Debug.Log("강제 복구: 공격 상태가 아니라서 IsAttacking 해제");
        }
    }

    public void AttackPlayer()
    {
        NKY_SoundManager.Instance.PlaySFX("NormalAttack");
        
        if (player == null || isDead) return;
        if (bossFollow != null && bossFollow.IsDashingNow) return;

        if (playerHealth == null)
        {
            playerHealth = player.GetComponent<Health>();
        }

        if (playerHealth != null && playerHealth.IsDestroyed) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > attackRange) return;
        
        
        if (playerHealth != null)
        {
            DamageData damageData = DamageData.Create(null, damage);
            playerHealth.GetDamage(damageData);
        }
    }

    public void SetDead()
    {
        isDead = true;
        IsAttacking = false;
        health?.SetDamageable(true);
    }
}