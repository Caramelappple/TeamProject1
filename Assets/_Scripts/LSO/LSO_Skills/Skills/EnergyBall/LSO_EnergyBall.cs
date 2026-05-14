using UnityEngine;

public class LSO_EnergyBall : MonoBehaviour
{
    private int _damage = 100;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health health) && collision.CompareTag("Enemy"))
        {
            DamageData damage = new DamageData(health,_damage);
            health.GetDamage(damage);
        }
    }
    
    private void FixedUpdate()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Animation-magic-4-c") && stateInfo.normalizedTime >= 0.95f)
        {
            Destroy(gameObject);
        }
    }
}
