using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class LSO_PlayerMovement : MonoBehaviour
{
    public static LSO_PlayerMovement instance;

    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    public Health Health { get; private set; }
    public float speed = 3;
    private Animator _animator;
    private SpriteRenderer _sprite;
    private AudioSource _audioSource;
    
    [SerializeField]private AudioClip[] clips; //오디오 소리를 모아두는 클립
    [SerializeField]private float footstepRate = 0.2f; // 간격
    private float _footStepTime; // 시간받아서
    private int _clipIndex;
     
    [SerializeField] private bool canMove = true;

    private Vector2 _moveDir;
    private Vector2 _fixedLastDir = Vector2.down;
    private Vector2 _lastDir = Vector2.down;
    private Rigidbody2D _rigid;

    LSO_SkillItem _skillItem;
    private LSO_ISkill _skill;
    public Action<GameObject>[] OnSkillEvent;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        Health = GetComponent<Health>();
        _audioSource = GetComponent<AudioSource>();
    }


    private void Start()
    {
        OnSkillEvent = new Action<GameObject>[LSO_SkillSlot.instance.slotIndex];
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        _rigid.linearVelocity = _moveDir.normalized * speed;
    }

    private void Update()
    {
        if (OnSkillEvent == null || !canMove) return;
        if (Time.time - _footStepTime > footstepRate && _rigid.linearVelocity.magnitude > 0.1f)
        {
            _footStepTime = Time.time; //시간을 현재시간으로
            _audioSource.PlayOneShot(clips[_clipIndex]); // 오디오 실행
            _clipIndex = (_clipIndex + 1) % clips.Length;
        }

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
        if (!canMove) //움직일수 없을떼
        {
            _moveDir = Vector2.zero;
            return;
        }

        _moveDir = value.Get<Vector2>();
        if (_moveDir != Vector2.zero)
        {
            //움직였을때
            _fixedLastDir = _moveDir;
            _lastDir = _moveDir;
        }

        if (_moveDir.x != 0) //보는 방향 따라서 뒤집기
        {
            _sprite.flipX = _moveDir.x < 0;
        }

        if (_fixedLastDir.x != 0 && _fixedLastDir.y != 0) //대각선으로 움직였을때 양옆으로 변환해주기
        {
            _fixedLastDir = new Vector2(Mathf.Sign(_fixedLastDir.x), 0);

            _animator.SetFloat(MoveY, 0);
            _animator.SetBool(MoveX, _moveDir.x != 0);
        }
        else
        {
            _animator.SetFloat(MoveY, _moveDir.y);
            _animator.SetBool(MoveX, _moveDir.x != 0);
        }

        /*if (Time.time - _footStepTime > footstepRate) // 시간을 재준다.
        {
            _footStepTime = Time.time; //시간을 현재시간으로
            _audioSource.PlayOneShot(clips[_clipIndex]); // 오디오 실행
            _clipIndex = (_clipIndex + 1) % clips.Length;
        }*/
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<LSO_SkillItem>(out LSO_SkillItem touchedSkill))
        {
            _skillItem = touchedSkill;
        }
    }

    public void SetMove(bool move)
    {
        canMove = move;
        if (!canMove)
        {
            _moveDir = Vector2.zero;
            if (_rigid != null) _rigid.linearVelocity = Vector2.zero;
            _animator.SetBool(MoveX, false);
            _animator.SetFloat(MoveY, 0);
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

                if (_moveDir != Vector2.zero)
                {
                    //움직였을때
                    _fixedLastDir = _moveDir;
                    _lastDir = _moveDir;
                }

                _animator.SetFloat(MoveY, _moveDir.y);
                _animator.SetBool(MoveX, _moveDir.x != 0);
                if (_fixedLastDir.x != 0 && _fixedLastDir.y != 0) //대각선으로 움직였을때
                {
                    _fixedLastDir = new Vector2(Mathf.Sign(_fixedLastDir.x), 0);

                    _animator.SetFloat(MoveY, 0);
                    _animator.SetBool(MoveX, _moveDir.x != 0);
                }
                else
                {
                    _animator.SetFloat(MoveY, _moveDir.y);
                    _animator.SetBool(MoveX, _moveDir.x != 0);
                }

                if (_moveDir.x != 0)
                {
                    _sprite.flipX = _moveDir.x < 0;
                }
            }
        }
    }

    public Vector3 GetFixedLastDir()
    {
        return _fixedLastDir;
    }

    public Vector3 GetLastDir()
    {
        return _lastDir.normalized;
    }

    public Vector3 GetMoveDir()
    {
        return _moveDir;
    }
}

