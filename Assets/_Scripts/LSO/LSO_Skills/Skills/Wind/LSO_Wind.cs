using System.Collections.Generic;
using UnityEngine;

public class LSO_Wind : MonoBehaviour
{
    private Rigidbody2D _rigid;
    
    private float _speed = 1f;
    private float _innerRange = 8f;
    [SerializeField] private int _damage = 3;
    private float _liveTime = 9.2f;
    private float _interval = 0.5f;
    private Health _playerHealth;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(Health health)
    {
        _playerHealth = health;
    }
    private void OnEnable()
    {
        Destroy(gameObject, _liveTime);
    }

    private void FixedUpdate()
    {
        if (!_playerHealth) return;
        Pull();
    }

    private void Pull()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _innerRange);

        GameObject nearest = null;
        float minDist = float.MaxValue;

        foreach (Collider2D target in targets)
        {
            if (target == null) continue;
            if (!target.CompareTag("Enemy")) continue;
            
            _rigid.linearVelocity = Vector2.zero;

            Vector2 dir = (Vector2)transform.position - (Vector2)target.transform.position;
            float dist = dir.magnitude;

            // AddForce로 흡입 (Tween 안 씀)

            // 가장 가까운 Enemy 추적
            if (dist < minDist)
            {
                minDist = dist;
                nearest = target.gameObject;
            }
        }

        // 토네이도는 가장 가까운 Enemy 추적
        if (nearest != null)
        {
            Vector2 dir = (nearest.transform.position - transform.position).normalized;
            _rigid.linearVelocity = dir * _speed;
        }
        else
        {
            _rigid.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Health>(out Health health) && collision.CompareTag("Enemy"))
        {
            DamageData data = new DamageData(health, _damage);
            health.GetDamage(data);
        }
    }
    
    private void OnDisable()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _innerRange);

        foreach (Collider2D target in targets)
        {
            if (target == null) continue;
            if (!target.CompareTag("Enemy")) continue;

            Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
            if (targetRb == null) continue;

            targetRb.linearVelocity = Vector2.zero; // 남은 힘 초기화
        }
    }
    
    private Dictionary<GameObject, float> _timer = new Dictionary<GameObject, float>();
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_playerHealth ) return;
        _timer.TryAdd(collision.gameObject, 0f);

        _timer[collision.gameObject] += Time.deltaTime;

        if (_timer[collision.gameObject] >= _interval)
        {
            if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health health))
            {
                DamageData data = new DamageData(_playerHealth, _damage);
                health.GetDamage(data);
            }

            _timer[collision.gameObject] = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 나가면 타이머 제거 (중요)
        _timer.Remove(other.gameObject);
    }
}