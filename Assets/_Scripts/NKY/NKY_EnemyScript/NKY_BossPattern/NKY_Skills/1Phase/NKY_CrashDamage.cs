using UnityEngine;

namespace _Scripts.NKY.NKY_EnemyScript.NKY_Skills
{
    public class NKY_CrashDamage : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask;
        private Collider2D col;
        public int damage;
    
        private ContactFilter2D filter;

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            filter = new ContactFilter2D();
            filter.SetLayerMask(layerMask);
            filter.useTriggers = true;
        
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Health>(out Health health))
            {
                
                DamageData damageData = DamageData.Create(health, damage);
                health.GetDamage(damageData);
            }
        }
    }
}
