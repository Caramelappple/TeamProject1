using DG.Tweening;
using UnityEngine;

public class KHG_CircleBullet : MonoBehaviour
{
    private readonly string _enemyTag = "Enemy";
    private float _speed;
    private float _trackingSpeed = 0.8f;
    private float _liveTime = 3f;

    private Transform _target;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    [SerializeField] private int damage = 5;
    [SerializeField] private float scanRange = 3;


    [SerializeField] private Collider2D[] targets;
    [SerializeField] private Transform nearestTarget;
    private Health _playerHealth;

    bool HasTarget => _target != null;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void Init(Health playerHealth,float speed)
    {
        _playerHealth = playerHealth;
        _speed =  speed;
        Destroy(gameObject, _liveTime);
    }

    Transform GetNearest()
    {
        Transform result = null;
        float nearestDist = float.MaxValue;

        foreach (Collider2D col in targets)
        {
            if (col.transform == transform || !col.CompareTag("Enemy")) continue;

            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                result = col.transform;
            }
        }

        return result;
    }

    private void FixedUpdate()
    {
        if (!_playerHealth || _speed == 0) return;
        targets = Physics2D.OverlapCircleAll(transform.position, scanRange);
        nearestTarget = GetNearest();
        
        _target = nearestTarget;

        if (!HasTarget) return;

        Move();
        FlipX();
        
        if (_animator != null)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Animation-magic-5-a") && stateInfo.normalizedTime >= 0.95f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Move()
    {
        if (!_target || !_target.CompareTag("Enemy")) return;

        // Vector2.zero 제거
        Vector2 targetVelocity = (_target.position - transform.position).normalized * _speed;
        _rb.linearVelocity = Vector2.Lerp(_rb.linearVelocity, targetVelocity, _trackingSpeed * Time.fixedDeltaTime);
    }

    private void FlipX()
    {
        if (_rb.linearVelocity.x != 0)
            _spriteRenderer.flipX = _rb.linearVelocity.x < 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag) && collision.TryGetComponent(out Health health) && _playerHealth)
        {
            DamageData data = new DamageData(_playerHealth, damage);
            health.GetDamage(data);
        }
    }
}