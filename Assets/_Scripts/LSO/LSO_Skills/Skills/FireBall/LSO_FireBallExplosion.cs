using System;
using UnityEngine;

public class LSO_FireBallExplosion : MonoBehaviour
{
    private Animator _animator;
    private int _damage = 40;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health health))
        {
            DamageData data = new DamageData(health, _damage);
            health.GetDamage(data);
        }
    }

    private void FixedUpdate()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("FireBallExplosion") && stateInfo.normalizedTime >= 0.95f)
        {
            Destroy(gameObject);
        }
    }
}
