using UnityEngine;

public class BossHitReaction : MonoBehaviour
{
    private Health health;
    private Animator anim;
    private bool isDead;

    [SerializeField] private MonoBehaviour[] disableScripts;
    [SerializeField] private Collider2D[] disableColliders;
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (health != null)
            health.OnDamage += HandleDamage;
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnDamage -= HandleDamage;
    }

    private void HandleDamage(DamageResultData data)
    {
        if (anim == null || isDead) return;

        if (health.IsDestroyed)
        {
            Die();
            return;
        }

        anim.SetTrigger("TakeHit");
    }

    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Dead");

        if (health != null)
            health.SetDamageable(false);

        foreach (MonoBehaviour script in disableScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        foreach (Collider2D col in disableColliders)
        {
            if (col != null)
                col.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }
    }
}