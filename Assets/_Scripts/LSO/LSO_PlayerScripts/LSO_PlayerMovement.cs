using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class LSO_PlayerMovement : MonoBehaviour
{
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    public Health Health {get; private set;}
    public float speed = 3;
    protected Animator Animator;
    private SpriteRenderer _sprite;
    
    [SerializeField]private bool _canMove = true;

    private int _disableMoveCount = 0; // 김동휘가 추가함

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
        Health = GetComponent<Health>();
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
        // false 이면 카운트 증가, true이면 감소
        if (!move) // 김동휘가 추가함
        {
            _disableMoveCount++; // 김동휘가 추가함
        }
        else
        {
            _disableMoveCount--; // 김동휘가 추가함
            if (_disableMoveCount < 0) _disableMoveCount = 0; // 0 이하로 떨어지지 않게 방어
        }

        // 이동 금지 카운트가 0일 때만 실제로 이동 가능 상태(_canMove = true)가 됨
        _canMove = (_disableMoveCount == 0); // 김동휘가 추가함 
        ////////여기까지.//////////
        
        // 다른 이동기 스킬 만들 때 그냥 SetMove(false)와 SetMove(true) 그냥 상황에 맞춰서 쓰면 알아서 적용 됨.



        if (!_canMove)
        {
            _moveDir = Vector2.zero;
            if (_rigid != null) _rigid.linearVelocity = Vector2.zero;
            Animator.SetBool(MoveX, false);
            Animator.SetFloat(MoveY, 0);
            if (_moveDir.x != 0)
            {
                _sprite.flipX = _moveDir.x < 0;
            }
        }
        else
        {
            if (Keyboard.current != null)
            {
                Vector2 dir = Vector2.zero;
                if (Keyboard.current.wKey.isPressed) dir.y += 1;
                if (Keyboard.current.sKey.isPressed) dir.y -= 1;
                if (Keyboard.current.aKey.isPressed) dir.x -= 1;
                if (Keyboard.current.dKey.isPressed) dir.x += 1;
                _moveDir = dir;

                if (_moveDir != Vector2.zero) //움직였을때
                    _lastDir = _moveDir;

                Animator.SetFloat(MoveY, _moveDir.y);
                Animator.SetBool(MoveX, _moveDir.x != 0);

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
        }
    }

    public Vector3 GetLastDir()
    {
        return _lastDir;
    }
// base
}

