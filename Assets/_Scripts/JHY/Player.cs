using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 7f;
    private Vector2 moveDir;
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
}
