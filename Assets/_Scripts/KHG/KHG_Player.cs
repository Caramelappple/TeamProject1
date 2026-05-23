using System;
using KDH.ItemSystem;
using UnityEngine;

namespace KHG.Player
{
    public class KHG_Player : MonoBehaviour
    {
        public Health Health { get; private set; }
        
        private void Awake()
        {
            Health = GetComponent<Health>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out KHG_ICollectable iCollectable))
            {
                Debug.Log($"[플레이어] 트리거 아이템 발견: {other.gameObject.name}");
                iCollectable.Collect(this);
            }
        }

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