using UnityEngine;

namespace KSY.Enemy
{
    public class KSY_Enemy : MonoBehaviour
    {
        private int _damage = 1;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            KSY_Health health;

            if (collision.gameObject.TryGetComponent<KSY_Health>(out health))
            {
                health.OnDamage += Test1;

                health.GetDamage(_damage, gameObject);
                Debug.Log(gameObject.name);

                health.OnDamage -= Test1;
            }
        }
        public void Test1(KSY_DamageEventArgs args)
        {
            int damage = args.damage;
            int currentHealth = args.currentHealth;

            args.damage = 10;

            Debug.Log($"{damage}만큼 맞았어요 현재 체력 : {currentHealth}");
        }
    }
}