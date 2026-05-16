using System;
using KHG.HealthSystme;
using KHG.Player;
using UnityEngine;
using KHG.ItemSystem;

public class Healpack : MonoBehaviour, KHG_ICollectable
{
    [SerializeField] private int healAmount = 5;
    public event Action<KHG_ICollectable> OnCollected;

    public void Collect(KHG_Player collector)
    {
        var health = collector.Health;
        bool hasHealth = health != null;

        if (hasHealth)
        {
            KHG_RecoverData data =  new KHG_RecoverData(null, healAmount);
            health.Recover(data);
        }
    }
}
