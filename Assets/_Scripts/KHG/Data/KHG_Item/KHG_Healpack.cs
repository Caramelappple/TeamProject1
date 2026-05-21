using System;
using _Scripts.HealthSystem;
using KHG.Player;
using UnityEngine;
using KHG.ItemSystem;

public class KHG_Healpack : MonoBehaviour, KHG_ICollectable
{
    [SerializeField] private int healAmount = 5;
    public event Action<KHG_ICollectable> OnCollected;
    private void OnTriggerEnter2D(Collider2D other)
    {
        KHG_Player player = other.GetComponent<KHG_Player>();
        if (player != null) 
            Collect(player);
    }

    public void Collect(KHG_Player collector)
    {
        var health = collector.Health;
        bool hasHealth = health != null;

        if (hasHealth)
        {
            RecoverData data = new RecoverData(null, healAmount);
            health.Recover(data);
            OnCollected?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
