using UnityEngine;

public class KHG_CircleBullet : MonoBehaviour
{
    private readonly string _enemyTag = "Enemy";
    public float speed = 7f;

    private Transform _target;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Health _playerHealth;
    [SerializeField] private int damage = 5;
    private Animator _animator;
    
    [SerializeField] private float scanRange;
    [SerializeField] private RaycastHit2D[] targets;
    [SerializeField] private Transform nearestTarget;

    bool hasTarget
    {
        get { return _target != null; }
    }
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
  
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff<diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, 0);
        nearestTarget = GetNearest();
        
        if(!hasTarget) return;
        Move();
        FlipX();
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Animation-magic-5-a") && stateInfo.normalizedTime >= 0.95f)
        {
            Destroy(gameObject);
        }
        
        _target = nearestTarget;
        if (_target = null)
        {
            Move();
        }
    }

    private void Move()
    {
        _rb.linearVelocity = Vector2.zero;
        Vector2 direction = (_target.position - transform.position).normalized;
        _rb.linearVelocity = direction * speed;
    }

    private void FlipX()
    {
        if (_rb.linearVelocity.x != 0)
            _spriteRenderer.flipX = (_rb.linearVelocity.x < 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health health))
        {
            DamageData data = new DamageData(_playerHealth, damage);
            health.GetDamage(data);
        }
    }
    
}