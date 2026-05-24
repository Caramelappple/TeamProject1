using UnityEngine;

public class LSO_TLemp : MonoBehaviour
{
    private Health _enemyHealth;

    private void Awake()
    {
        _enemyHealth = GetComponent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Health>(out Health health))
        {
            DamageData data = new DamageData(_enemyHealth,10);
            health.GetDamage(data);
        }
    }
}