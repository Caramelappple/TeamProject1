using System;
using _Scripts.HealthSystem;
using Unity.VisualScripting;
using UnityEngine;

public class NKY_Health : NKY_DamageableResources, NKY_IRecoverable
{
    public event Action<NKY_RecoverResultData> OnRecover;

    public void Recover(NKY_RecoverData data)
    {
        if(IsDestroyed) return;

        int recoverValue = data.recoverValue;
        NKY_Health giver = data.giver;

        int lastValue = Value;
        Value += recoverValue;
        int calcValue = Value;
        bool hasRecover = lastValue < calcValue;

        if(hasRecover)
        {
            Debug.Log($"{recoverValue}??? ???? ???????. ??????? : {calcValue}");
            NKY_RecoverResultData resultData = NKY_RecoverResultData.Create(giver, recoverValue, Value);
            OnRecover?.Invoke(resultData);
        }


    }
    
    //??? ??
    [ContextMenu("Recover")]
    public void Recover()
    {
        NKY_RecoverData data = NKY_RecoverData.Create(null, 1);
        if (IsDestroyed) return;

        int recoverValue = data.recoverValue;

        int lastValue = Value;
        Value += recoverValue;
        int calcValue = Value;
        bool hasRecover = lastValue < calcValue;

        if (hasRecover)
        {
            NKY_RecoverResultData resultData = NKY_RecoverResultData.Create(data.giver, recoverValue, calcValue);
            OnRecover?.Invoke(resultData);
        }
    }
    
}