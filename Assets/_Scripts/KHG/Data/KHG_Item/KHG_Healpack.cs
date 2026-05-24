using System;
using _Scripts.HealthSystem;
using KHG.ItemSystem;
using KHG.Player;
using UnityEngine;

public class KHG_Healpack : MonoBehaviour, KHG_ICollectable
{
    [SerializeField] private int healAmount = 5;
    public event Action<KHG_ICollectable> OnCollected;

    public void Collect(KHG_Player collector)
    {
        var health = collector.Health;
        bool hasHealth = health != null;

        if (hasHealth)
        {
            RecoverData data = new RecoverData(null, healAmount);
            health.Recover(data);

            Debug.Log($"<color=green>[힐팩 획득 성공]</color> 힐량: {healAmount}");

            OnCollected?.Invoke(this);
            
            
            
        }
        else
        {
            Debug.LogWarning("[힐팩 에러] 플레이어와 부딪혔으나, 플레이어에게 Health 컴포넌트가 없습니다!");
        }
    }
}