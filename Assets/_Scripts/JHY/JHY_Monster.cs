using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Monster : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 2f;
    private Transform player;
    private Rigidbody2D rb;

    [Header("Attack")]
    [SerializeField] private int damage = 3;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime;

    [Header("Health")]
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D[] disableColliders;
    private bool isDead;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 4f;
    [SerializeField] private float knockbackDuration = 0.15f;
    private float knockbackTimer;

    [Header("Death Effect")]
    [SerializeField] private GameObject deathEffectPrefab;
    [SerializeField] private float deathEffectLifeTime = 1.5f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        if (target != null)
        {
            player = target.transform;
        }
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnDamage += HandleDamage;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDamage -= HandleDamage;
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            return;
        }

        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void TryDamagePlayer(Collider2D other)
    {
        if (isDead) return;
        if (!other.CompareTag("Player")) return;
        if (Time.time < lastAttackTime + attackCooldown) return;

        Health playerHealth = other.GetComponent<Health>();
        if (playerHealth == null)
        {
            playerHealth = other.GetComponentInParent<Health>();
        }

        if (playerHealth == null) return;

        DamageData data = DamageData.Create(null, damage);
        playerHealth.GetDamage(data);
        lastAttackTime = Time.time;
        //몬스터가 플레이어를 들이받거나 공격하는 소리 재생
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamagePlayer(other);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamagePlayer(collision.collider);
    }

    private void HandleDamage(DamageResultData data)
    {
        if (isDead) return;
        if (health == null) return;
        //몬스터가 대미지를 입었을 때
        ApplyKnockback();

        if (health.IsDestroyed)
        {
            Die();
        }
    }

    private void ApplyKnockback()
    {
        if (player == null) return;

        Vector2 dir = ((Vector2)transform.position - (Vector2)player.position).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        knockbackTimer = knockbackDuration;
    }

    private void Die()
    {
        isDead = true;
        NKY_SoundManager.Instance.PlaySFX("MobDead");
        rb.linearVelocity = Vector2.zero;

        if (animator != null)
        {
            animator.SetTrigger("Dead");
        }

        foreach (Collider2D col in disableColliders)
        {
            col.enabled = false;
        }

        if (deathEffectPrefab != null)
        {
            GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectLifeTime);
        }

        Destroy(gameObject);
    }
    
}