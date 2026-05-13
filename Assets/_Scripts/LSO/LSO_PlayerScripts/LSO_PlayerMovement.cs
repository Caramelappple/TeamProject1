using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class LSO_PlayerMovement : MonoBehaviour
{
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    public float speed = 3;
    protected Animator Animator;
    private SpriteRenderer _sprite;
    
    [SerializeField]private bool _canMove = true;

    private Vector2 _moveDir;
    private Vector2 _lastDir = Vector2.down;
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
        if (!_canMove) return;
        _rigid.linearVelocity = _moveDir.normalized * speed;
    }

    private void Update()
    {
        if (OnSkillEvent == null || !_canMove) return;

        if (Keyboard.current.fKey.isPressed && _skillItem)
        {
            LSO_SkillSlot.instance.AddSkill(_skillItem, 0);
        }

        if (Keyboard.current.rKey.isPressed && _skillItem)
        {
            LSO_SkillSlot.instance.AddSkill(_skillItem, 1);
        }
        
        if (Keyboard.current.qKey.isPressed)
        {
            OnSkillEvent[0]?.Invoke(gameObject);
        }

        if (Keyboard.current.eKey.isPressed)
        {
            OnSkillEvent[1]?.Invoke(gameObject);
        }
    }

    private void OnMove(InputValue value)
    {
        if (!_canMove)
        {
            _moveDir = Vector2.zero;
            return;
        }
        
        Debug.Log(_rigid.linearVelocity);
        
        _moveDir = value.Get<Vector2>();
        if (_moveDir != Vector2.zero) //움직였을때
            _lastDir = _moveDir;
        
        if (_moveDir.x != 0)
        {
            _sprite.flipX = _moveDir.x < 0;
        }

        if (_lastDir.x != 0 && _lastDir.y != 0) //대각선으로 움직였을때
        {
            _lastDir = new Vector2(Mathf.Sign(_lastDir.x), 0);

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

 //HEAD

    public void SetMove(bool move)
    {
        _canMove = move;
    
        // 스턴이 시작되는 순간 즉시 속도와 입력값을 0으로 밀어버림
        if (!_canMove)
        {
            _moveDir = Vector2.zero;
            if (_rigid != null)
            {
                _rigid.linearVelocity = Vector2.zero;
            }
            Animator.SetBool(MoveX, false);
            Animator.SetFloat(MoveY, 0);
        }
    }
    
    public Vector3 GetLastDir()
    {
        return _lastDir;
    }
// base
}

