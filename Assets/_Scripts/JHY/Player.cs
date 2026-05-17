using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 7f;
    private Vector2 moveDir;
    private float currentSlowMultiplier = 1f;
    [SerializeField] private int damage = 5;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetSlow(float multiplier)
    {
        currentSlowMultiplier = multiplier;
    }

    public void ClearSlow()
    {
        currentSlowMultiplier = 1f;
    }
    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = moveDir * speed*currentSlowMultiplier;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Boss") && !collision.collider.CompareTag("Monster")) return;

        Health targetHealth = collision.collider.GetComponent<Health>();
        if (targetHealth == null)
        {
            targetHealth = collision.collider.GetComponentInParent<Health>();
        }

        if (targetHealth == null) return;

        DamageData data = DamageData.Create(null, damage);
        targetHealth.GetDamage(data);
    }


}