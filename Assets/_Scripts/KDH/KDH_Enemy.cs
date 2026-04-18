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
                
            }
        }
        public void Hit(int damage, GameObject giver)
        {
            Debug.Log($"나 피{damage} 만큼 달았고 때린놈은{giver}");
        }
    }
}