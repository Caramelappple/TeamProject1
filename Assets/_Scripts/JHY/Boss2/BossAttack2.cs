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