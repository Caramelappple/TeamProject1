using UnityEngine;

public class LSO_Wind : MonoBehaviour
{
    private Rigidbody2D _rigid;
    
    private float _speed = 1f;
    private float _pullForce = 10f;
    private float _innerRange = 5f;
    private int _damage = 30;
    private float _liveTime = 5f;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Destroy(gameObject, _liveTime);
    }

    private void FixedUpdate()
    {
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

            Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
            if (targetRb == null) continue;

            Vector2 dir = (Vector2)transform.position - targetRb.position;
            float dist = dir.magnitude;
            float strength = 1f - (dist / _innerRange); // 가까울수록 강하게

            // AddForce로 흡입 (Tween 안 씀)
            targetRb.AddForce(dir.normalized * (_pullForce * strength));

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
}