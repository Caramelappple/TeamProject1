using Assets._Scripts.NKY;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class NKY_Health : NKY_DamageableResources, NKY_IRecoverable
{
    public event Action<NKY_RecoverResultData> OnRecovery;

    [ContextMenu("Recover")]
    public void Recover()
    {
        NKY_RecoverData data = NKY_RecoverData.Create(null, 1);
        Recover(data);
    }
    public void Recover(NKY_RecoverData data)
    {
        if(IsDestroyed) return;

        int recoverValue = data.recoverValue;
        Entity giver = data.giver;

        int lastValue = Value;
        Value += recoverValue;
        int calcValue = Value;
        bool hasRecover = lastValue < calcValue;

        if(hasRecover)
        {
            Debug.Log($"{recoverValue}만큼 힐을 받았습니다. 현재채력 : {calcValue}");
            NKY_RecoverResultData resultData = NKY_RecoverResultData.Create(giver, recoverValue, Value);
            OnRecovery?.Invoke(resultData);
        }


    }

    private void Start()
    {
        SetDamageable(true);
        OnHit += Hit;
        OnDamage += TakeDamage;
    }

    private void Hit(NKY_DamageData data)
    {
        
    }
    private void TakeDamage(NKY_DamageResultData data)
    {

    }
}