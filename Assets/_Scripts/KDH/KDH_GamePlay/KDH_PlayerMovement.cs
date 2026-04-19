using UnityEngine;
using UnityEngine.InputSystem;

public class KDH_PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Vector2 _moveDir;
    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigid.linearVelocity = _moveDir * _speed;
    }

    private void OnMove (InputValue value)
    {
        _moveDir = value.Get<Vector2>();
    }
}