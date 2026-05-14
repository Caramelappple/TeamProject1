using KHG.HealthSystme;
using KHG.Player;
using UnityEngine;

public class Healpack : MonoBehaviour, KHG_ICollectable
{
    [SerializeField] private int healAmount = 5;
    public void Collect(KHG_Player collector)
    {
        bool hasHealth =  collector.TryGetComponent(out KHG_Health health);

        if (hasHealth)
        {
            KHG_RecoverData data =  new KHG_RecoverData(null, healAmount);
            health.Recover(data);
        }
    }
}
