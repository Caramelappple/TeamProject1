using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDir;
    [SerializeField] private float speed;
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
