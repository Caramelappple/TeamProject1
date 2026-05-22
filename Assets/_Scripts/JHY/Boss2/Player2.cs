using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 7f;
    private Vector2 moveDir;
    [SerializeField] private int damage = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDir * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            Health bossHealth = collision.gameObject.GetComponent<Health>();
            if (bossHealth != null)
            {
                DamageData damageData = DamageData.Create(null, damage);
                bossHealth.GetDamage(damageData);
            }
        }
    }
}