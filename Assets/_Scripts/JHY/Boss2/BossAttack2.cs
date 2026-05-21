using UnityEngine;
public class BossAttack2 : MonoBehaviour
{
    public Transform player;
    public float attackRange = 3f;
    public float attackCooldown = 3f;
    public int damage = 5;

    private Animator anim;
    private Health health;
    private BossMove2 bossFollow;
    private float lastAttackTime;
    private bool isDead;

    public bool IsAttacking { get; private set; }

    private void Start()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        bossFollow = GetComponent<BossMove2>();
        lastAttackTime = -attackCooldown;

        player = NKY_GameManager.instance.player.transform;
    }

    private void Update()
    {
        if (player == null || isDead) return;

        if (bossFollow != null && bossFollow.IsDashingNow) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            anim.SetTrigger("Attack");
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

    public void AttackPlayer()
    {
        if (player == null || isDead) return;
        if (bossFollow != null && bossFollow.IsDashingNow) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > attackRange) return;

        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            DamageData damageData = DamageData.Create(null, damage);
            playerHealth.GetDamage(damageData);
        }
    }

    public void SetDead()
    {
        isDead = true;
        health?.SetDamageable(true);
    }
}