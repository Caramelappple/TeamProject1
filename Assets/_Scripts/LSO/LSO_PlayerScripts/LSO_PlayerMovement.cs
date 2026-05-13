using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class LSO_PlayerMovement : MonoBehaviour
{
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    public float speed = 3;
    protected Animator Animator;
    private SpriteRenderer _sprite;

    private Vector2 _moveDir;
    public Vector2 lastDir = Vector2.down;
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
        if (_moveDir != Vector2.zero) //움직였을때
            lastDir = _moveDir;

        if (_moveDir.x != 0)
        {
            _sprite.flipX = _moveDir.x < 0;
        }

        if (lastDir.x != 0 && lastDir.y != 0) //대각선으로 움직였을때
        {
            lastDir = new Vector2(Mathf.Sign(lastDir.x), 0);
            Debug.Log(lastDir);

            Animator.SetFloat(MoveY, 0);
            Animator.SetBool(MoveX, _moveDir.x != 0);
        }
        else
        {
            Animator.SetFloat(MoveY, _moveDir.y);
            Animator.SetBool(MoveX, _moveDir.x != 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<LSO_SkillItem>(out LSO_SkillItem touchedSkill))
        {
            _skillItem = touchedSkill;
        }
    }

}

