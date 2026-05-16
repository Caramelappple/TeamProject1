using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Monster : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 3f;
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

        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;
        if (!collision.collider.CompareTag("Player")) return;
        if (Time.time < lastAttackTime + attackCooldown) return;

        Health playerHealth = collision.collider.GetComponent<Health>();
        if (playerHealth == null)
        {
            playerHealth = collision.collider.GetComponentInParent<Health>();
        }

        if (playerHealth == null) return;

        DamageData data = DamageData.Create(null, damage);
        playerHealth.GetDamage(data);
        lastAttackTime = Time.time;
    }

    private void HandleDamage(DamageResultData data)
    {
        if (isDead) return;
        if (health == null) return;
        if (!health.IsDestroyed) return;

        Die();
    }

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;

        if (animator != null)
        {
            animator.SetTrigger("Dead");
        }

        foreach (Collider2D col in disableColliders)
        {
            col.enabled = false;
        }

        Destroy(gameObject);
    }
}
