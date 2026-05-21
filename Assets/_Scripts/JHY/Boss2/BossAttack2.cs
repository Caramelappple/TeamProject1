using UnityEngine;

public class BossAttack2 : MonoBehaviour
{
    public Transform player;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int damage = 3;

    private Animator anim;
    private float lastAttackTime;
    private bool isDead;

    // ★ 추가됨: 보스가 현재 공격 애니메이션 중인지 확인하는 변수 (외부에서 읽기 전용)
    public bool IsAttacking { get; private set; }

    private void Start()
    {
        anim = GetComponent<Animator>();
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

    // ★ 추가됨: 애니메이션 이벤트가 맨 처음(0프레임)에 호출할 함수
    public void OnAttackStart()
    {
        IsAttacking = true;
    }

    // ★ 추가됨: 애니메이션 이벤트가 맨 끝(마지막 프레임)에 호출할 함수
    public void OnAttackEnd()
    {
        IsAttacking = false;
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
    }
}