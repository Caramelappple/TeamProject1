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
                health.OnHit += Test2;

                health.GetDamage(_damage, gameObject);
                Debug.Log(gameObject.name);

                health.OnDamage -= Test1;
                health.OnHit -= Test2;
            }
        }
        public void Test1(DamageEventArgs args)
        {
            int damage = args.damage;
            int currentHealth = args.currentHealth;

            Debug.Log($"{damage}만큼 맞았어요 현재 체력 : {currentHealth}");
        }
        public void Test2(int damage, GameObject giver)
        {
            Debug.Log("나 피가 달았어요!");
        }
    }
}