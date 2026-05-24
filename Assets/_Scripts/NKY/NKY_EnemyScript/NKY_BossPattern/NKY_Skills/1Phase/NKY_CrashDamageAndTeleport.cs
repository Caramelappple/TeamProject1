using System;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;

namespace _Scripts.NKY.NKY_EnemyScript.NKY_Skills
{
    public class NKY_CrashDamageAndTeleport : NKY_PatternCoroutine
    {
        [SerializeField] private LayerMask layerMask;
        public Transform teleportTarget;
        public int damage;
    
        private ContactFilter2D filter;

        protected override void OnAwake()
        {
            filter = new ContactFilter2D();
            filter.SetLayerMask(layerMask);
            filter.useTriggers = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Health health))
            {
                Vector2 dir =  (teleportTarget.position - transform.position).normalized;
                if (dir.x < 0)
                {
                    teleportTarget.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    teleportTarget.rotation = Quaternion.Euler(0, 0, 0);
                }
                teleportTarget.position = transform.position;
                DamageData damageData = DamageData.Create(health, damage);
                health.GetDamage(damageData);
            }
        }
    }
}