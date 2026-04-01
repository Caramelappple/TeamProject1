using System;
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
                //플레이어에게 데미지를 1 줘보세요
                // <<
                health.OnHit = Test1;
                health.OnDamage = Test2;
                int asd = health.GetDamage(_damage);
                int cv = health.GetValue();

                // 상속의 가장 큰 특징 2가지
                // 1. 부모의 메서드, 변수를 사용할 수 있다.
                // 2. 부모로 형변환을 할 수 있다.
            }
        }
        public void Test1(int damage)
        {
            Debug.Log($"{damage}만큼 맞았어요");
        }
        public void Test2(int damage)
        {
            Debug.Log("나 피가 달았어요!");
        }
    }
}