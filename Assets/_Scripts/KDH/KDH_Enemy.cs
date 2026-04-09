using KDH.Player; //다른 네임스페이스를 가져올 때는 using을 사용하자.
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.GPUSort;

namespace KDH.Enemy
{
    public class KDH_Enemy : MonoBehaviour
    {
        private int _damage = 1;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            KDH_Health health;
            if (collision.gameObject.TryGetComponent(out health))
            {
                // '=' 할당:하나만 등록, "+=" 구독: 2개 이상 등록 가능
                health.OnDamaged += Damage;
                health.GetDamage(_damage, gameObject);
                Debug.Log(gameObject.name);
                // '-'를 더하면 빼는 거임 (하나만 호출, 헤제한다고 함)
                health.OnDamaged -= Damage;
            }
        }
        public void Hit(int damage, GameObject giver)
        {
            Debug.Log($"나 피{damage} 만큼 달았고 때린놈은{giver}");
        }
        public void Damage(KDH_DamageArgs args)
        {
            int damage = args.damage;
            int currentHealth = args.currentHealth;
            Debug.Log($"{damage}만큼 맞았어요 현재 체력: {currentHealth}");
        }
    }
}