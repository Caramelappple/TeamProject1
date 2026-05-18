using UnityEngine;

public class LSO_TLemp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Health>(out Health health))
        {
            DamageData data = new DamageData(health,10);
            health.GetDamage(data);
        }
    }
}