using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    [SerializeField] private float speed = 3;
    protected Animator Animator;
    private SpriteRenderer _sprite;
    private Vector2 _moveDir;
    protected Vector2 LastDir = Vector2.down;
    protected Vector2 FirstDir;
    private Rigidbody2D _rigid;
    
    private bool _canSkill = true;
    
    public event Action OnSkillEvent;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        _rigid.linearVelocity = _moveDir * speed;
        Animator.SetBool(MoveX, _moveDir.x != 0);

        if (Keyboard.current.spaceKey.isPressed && _canSkill)
        {
            OnSkillEvent?.Invoke();
        }
    }
    private void OnMove(InputValue value)
    {
        _moveDir = value.Get<Vector2>();
        if (_moveDir != Vector2.zero)
            LastDir = _moveDir;
        if (_moveDir.x != 0)
        {
            _sprite.flipX = _moveDir.x < 0;
        }
        Animator.SetFloat(MoveY, _moveDir.y);
    }
    
}

