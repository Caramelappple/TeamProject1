using System.Collections;
using UnityEngine;

public class LSO_Clone : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    [SerializeField] private GameObject sword;
    
    private bool _attackable = true;
    private readonly float _cooldown = 0.5f;
    private readonly float _attackTime = 0.2f;
    private readonly float _liveTime = 7.8f;
    [SerializeField] private readonly int _damage = 10;
    
    private bool _isAnimReady;

    private GameObject _player;
    private LSO_PlayerMovement _movement;
    private Health _playerHealth;
    
    private Vector3 _lastDir;
    private Animator _cloneAnim;
    private SpriteRenderer _clonSprite;

    private void Awake()
    {
        _cloneAnim = GetComponent<Animator>();
        _clonSprite = GetComponent<SpriteRenderer>();
        _cloneAnim.enabled = false;
        _clonSprite.enabled = false;
    }

    private void Start()
    {
        sword.SetActive(false);
    }

    private void OnEnable()
    {
        Destroy(gameObject, _liveTime);
    }

    private void Update()
    {
        if (!_attackable || !_player || !_movement || !_isAnimReady) return;
        _attackable = false;
        SetClone();
        StartCoroutine(Attack());
        gameObject.GetComponent<Rigidbody2D>().linearVelocity = _lastDir * 0.5f;
    }

    private IEnumerator Attack()
    {
        //_attackable = false;
        
        LSO_SoundManager.Instance.SfxPlay(clip);
        sword.SetActive(true);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(sword.transform.position, sword.transform.localScale / 2, 0);
        foreach (Collider2D collision in colliders)
        {
            if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health enemyHealth))
            {
                DamageData data = DamageData.Create(_playerHealth, _damage);
                enemyHealth.GetDamage(data);
            }
        }

        yield return new WaitForSeconds(_attackTime);

        sword.SetActive(false);
        yield return new WaitForSeconds(_cooldown);
        _attackable = true;
    }
    
    private void SetDir()
    {
        if (_lastDir.x != 0)
            _clonSprite.flipX = _lastDir.x < 0;

        if (_lastDir.x != 0 && _lastDir.y != 0)
        {
            _lastDir = new Vector2(Mathf.Sign(_lastDir.x), 0);
            _cloneAnim.SetFloat(MoveY, 0);
            _cloneAnim.SetBool(MoveX, true);
        }
        else
        {
            _cloneAnim.SetFloat(MoveY, _lastDir.y);
            _cloneAnim.SetBool(MoveX, _lastDir.x != 0);
        }
    }

    private void SetClone()
    {
        sword.transform.position = transform.position + _lastDir;
        _cloneAnim.SetTrigger("Attack");
    }

    public void Init(LSO_PlayerMovement movement)
    {
        _player = movement.gameObject;
        _playerHealth = _player.GetComponent<Health>();
        _movement = movement;
        _lastDir = movement.GetFixedLastDir();
        StartCoroutine(InitAnim());
    }

    private IEnumerator InitAnim()
    {
        yield return new WaitForSeconds(0.1f);
        _cloneAnim.enabled = true;
        SetDir();
        _clonSprite.enabled = true;
        _isAnimReady = true;
    }
}