using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    protected Animator _animator;
    private SpriteRenderer _spriter;
    protected Vector2 _moveDir;
    protected Vector2 _lastDir = Vector2.down;
    protected Vector2 _firstDir;
    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriter = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        _rigid.linearVelocity = _moveDir * _speed;
        _animator.SetBool("MoveX", _moveDir.x != 0);
    }
    private void OnMove(InputValue value)
    {
        _moveDir = value.Get<Vector2>();
        if (_moveDir != Vector2.zero)
            _lastDir = _moveDir;
        if (_moveDir.x != 0)
        {
            _spriter.flipX = _moveDir.x < 0;
        }
        _animator.SetFloat("MoveY", _moveDir.y);
        
        
    }
    
}

