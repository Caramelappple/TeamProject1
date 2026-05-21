using UnityEngine;

public class LSO_BeamBullet : MonoBehaviour
{
    private float _speed = 0.25f;
    private float _lifeTime = 1.8f;
    private int _damage = 5;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
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
