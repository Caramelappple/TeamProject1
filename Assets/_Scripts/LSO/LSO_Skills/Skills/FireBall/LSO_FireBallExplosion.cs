using System;
using UnityEngine;

public class LSO_FireBallExplosion : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    
    private Animator _animator;
    [SerializeField] private int _damage = 40;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void OnEnable()
    {
        LSO_SoundManager.Instance.SfxPlay(clip);
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
