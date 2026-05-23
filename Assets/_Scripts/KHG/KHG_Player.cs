using System;
using KDH.ItemSystem;
using UnityEngine;

namespace KHG.Player
{
    public class KHG_Player : MonoBehaviour
    {
        // 프로퍼티를 외부에서 읽을 수 있게 유지합니다.
        public Health Health { get; private set; }
        
        private void Awake()
        {
            // ⭐️ [추가] 시작할 때 본인 오브젝트에서 Health 컴포넌트를 자동으로 가져옵니다.
            Health = GetComponent<Health>();
        }

        // ⭐️ [추가] 아이템(힐팩)은 통과하면서 먹어야 하므로 Trigger를 사용합니다.
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out KHG_ICollectable iCollectable))
            {
                Debug.Log($"[플레이어] 트리거 아이템 발견: {other.gameObject.name}");
                iCollectable.Collect(this);
            }
        }

        // 기존에 쓰시던 딱딱한 물리 충돌(벽, 몬스터 등)도 유지되도록 남겨둡니다.
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out KHG_ICollectable iCollectable))
            {
                Debug.Log($"[플레이어] 물리 충돌 아이템 발견: {other.gameObject.name}");
                iCollectable.Collect(this);
            }
        }
    }   
}