using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float _speed = 3;
    private Vector2 _MoveDir;
    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        _rigid.linearVelocity = _MoveDir * _speed;
    }
    private void OnMove(InputValue value)
    {
        _MoveDir = value.Get<Vector2>();
    }

}

