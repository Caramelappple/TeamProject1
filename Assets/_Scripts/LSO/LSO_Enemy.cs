using LSO.Health;
using UnityEngine;

namespace LSO.Enemy
{
    public class LSO_Enemy : MonoBehaviour
    {
        private int _damage = 1;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            LSO_Health health;

            if (collision.gameObject.TryGetComponent<LSO_Health>(out health))
            {
                health.GetDamage(_damage);
                Debug.Log(health);
            }
        }
    }
}

