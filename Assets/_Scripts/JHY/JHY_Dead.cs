using UnityEngine;

public class JHY_Dead : MonoBehaviour
{
    [SerializeField] private Health health;
    private Animator ani;
    private bool isDead;

    [SerializeField] private MonoBehaviour[] disableScripts;
    [SerializeField] private Collider2D[] disableColliders;
    [SerializeField] private Rigidbody2D rb;
   
    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        health.OnDamage += HandleDamage;
    }

    private void OnDisable()
    {
        health.OnDamage -= HandleDamage;
    }

    private void HandleDamage(DamageResultData data)
    {
        if (isDead) return;

        if (health.IsDestroyed)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        JHY_Attack attack = GetComponent<JHY_Attack>();
        if (attack != null)
        {
            attack.ClearAura();
        }

        ani.SetTrigger("Dead");

        foreach (MonoBehaviour script in disableScripts)
        {
            script.enabled = false;
        }

        foreach (Collider2D col in disableColliders)
        {
            col.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }
    }
}
