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
    
    LSO_SkillItem _skillItem;
    private LSO_ISkill _skill;
    public Action<GameObject>[] OnSkillEvent;
    
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        //OnSkillEvent = new Action<GameObject>[LSO_SkillSlot.instance.slotIndex];
    }

    private void Start()
    {
        OnSkillEvent = new Action<GameObject>[LSO_SkillSlot.instance.slotIndex];
    }

    private void FixedUpdate()
    { 
        _rigid.linearVelocity = _moveDir * speed;
        Animator.SetBool(MoveX, _moveDir.x != 0);
    }

    private void Update()
    {
        if (OnSkillEvent == null) return;
        
        if (Keyboard.current.qKey.isPressed)
        {
            OnSkillEvent[0]?.Invoke(gameObject);
        }

        if (Keyboard.current.eKey.isPressed)
        {
            OnSkillEvent[1]?.Invoke(gameObject);
        }

        if (Keyboard.current.fKey.isPressed && _skillItem)
        {
            LSO_SkillSlot.instance.AddSkill(_skillItem, 0);
        }

        if (Keyboard.current.rKey.isPressed && _skillItem)
        {
            LSO_SkillSlot.instance.AddSkill(_skillItem, 1);
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
        if (collision.TryGetComponent<LSO_SkillItem>(out LSO_SkillItem touchedSkill))
        {
            _skillItem = touchedSkill;
        }
    }
}

