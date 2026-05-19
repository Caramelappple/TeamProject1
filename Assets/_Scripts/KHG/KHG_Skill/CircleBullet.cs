using UnityEngine;

public class CircleBullet : MonoBehaviour
{
    private readonly string _enemyTag = "Enemy";
    public float speed = 7f;

    private Transform _target;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Health _playerHealth;
    [SerializeField] private int damage = 5;
    private Animator _animator;

    bool hasTarget
    {
        get { return _target != null; }
    }
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        FindTarget(); 
    }

    private void FindTarget()
    {
        GameObject enemyObj = GameObject.FindWithTag(_enemyTag);
        if (enemyObj != null)
            _target = enemyObj.transform;
    }

    private void FixedUpdate()
    {
        if(!hasTarget) return;
        Move();
        FlipX();
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Animation-magic-5-a") && stateInfo.normalizedTime >= 0.95f)
        {
            Destroy(gameObject);
        }
        
    }

    private void Move()
    {
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