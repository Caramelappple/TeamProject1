using UnityEngine;
public class BossAttack2 : MonoBehaviour
{
    public Transform player;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int damage = 3;
    private Animator anim;
    private Health health; // ★ 추가
    private float lastAttackTime;
    private bool isDead;
    public bool IsAttacking { get; private set; }
    private void Start()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<Health>(); // ★ 추가
        lastAttackTime = -attackCooldown;
    }
    private void Update()
    {
        if (player == null || isDead) return;
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            anim.SetTrigger("Attack");
        }
    }
    // 애니메이션 이벤트 - 공격 시작 프레임
    public void OnAttackStart()
    {
        IsAttacking = true;
        health?.SetDamageable(false); // ★ 공격 중 피격 불가
    }
    // 애니메이션 이벤트 - 공격 끝 프레임
    public void OnAttackEnd()
    {
        IsAttacking = false;
        health?.SetDamageable(true); // ★ 공격 끝나면 피격 가능
    }
    public void AttackPlayer()
    {
        if (player == null || isDead) return;
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
        health?.SetDamageable(true); // ★ 사망 시 반드시 원복 (Die() 흐름 보장)
    }
}