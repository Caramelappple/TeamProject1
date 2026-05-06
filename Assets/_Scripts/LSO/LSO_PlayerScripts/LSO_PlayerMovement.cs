using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class LSO_PlayerMovement : MonoBehaviour
{
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    [SerializeField] private float speed = 3;
    protected Animator Animator;
    private SpriteRenderer _sprite;
    
    private Vector2 _moveDir;
    protected Vector2 LastDir = Vector2.down;
    private Rigidbody2D _rigid;
    private bool isDashing;
    
    SkillItem _skillItem;
    private ISkill _skill;
    public event Action OnSkillEvent1;
    public event Action OnSkillEvent2;

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
    }

    private void Update()
    {
        if (Keyboard.current.qKey.isPressed)
        {
            OnSkillEvent1?.Invoke();
        }

        if (Keyboard.current.eKey.isPressed)
        {
            OnSkillEvent2?.Invoke();
        }

        if (Keyboard.current.fKey.isPressed && _skillItem != null)
        {
            _skill = _skillItem._skill;
            SkillSlot.instance.AddSkill(_skill, 0);
        }

        if (Keyboard.current.rKey.isPressed && _skillItem != null)
        {
            _skill = _skillItem._skill;
            SkillSlot.instance.AddSkill(_skill, 1);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<SkillItem>(out SkillItem touchedSkill))
        {
            _skillItem = touchedSkill;
        }
    }
}

