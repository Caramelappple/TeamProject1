using UnityEngine;

public class JHY_Dead : MonoBehaviour
{
    [SerializeField] private float health = 50f;
    private Animator ani;
    private bool isDead;

    [SerializeField] private MonoBehaviour[] disableScripts;
    [SerializeField] private Collider2D[] disableColliders;
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        health -= 1;
        Debug.Log(health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
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
