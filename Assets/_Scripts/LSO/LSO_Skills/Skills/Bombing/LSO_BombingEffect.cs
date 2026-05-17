using UnityEngine;

public class LSO_BombingEffect : MonoBehaviour
{
    private int _damage = 200;
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
        if (stateInfo.IsName("Bombing") && stateInfo.normalizedTime >= 0.95f)
        {
            Destroy(gameObject);
        }
    }
}