using System;
using UnityEngine;

public class LSO_BeamBullet : MonoBehaviour
{
    private Rigidbody2D _rigid;
    private float _speed = 0.25f;
    private float _lifeTime = 1.8f;
    private int _damage = 2;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        transform.position += transform.right * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health health))
        {
            DamageData data = new DamageData(health, _damage);
            health.GetDamage(data);
        }
    }
}
