using System;
using UnityEngine;

public class CrashDamage : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    private Collider2D col;
    [SerializeField] private int damage;
    
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
        if (collision.TryGetComponent<NKY_Health>(out NKY_Health health))
        {
            NKY_DamageData damageData = NKY_DamageData.Create(health, damage);
            health.GetDamage(damageData);
        }
    }
}
