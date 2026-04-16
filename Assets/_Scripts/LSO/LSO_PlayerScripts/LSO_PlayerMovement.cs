using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    protected Vector2 _moveDir;
    protected Vector2 _lastDir = Vector2.down;
    protected Vector2 _firstDir;
    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        _rigid.linearVelocity = _moveDir * _speed;
    }
    private void OnMove(InputValue value)
    {
        _moveDir = value.Get<Vector2>();
        if (_moveDir != Vector2.zero)
            _lastDir = _moveDir;
    }

}

