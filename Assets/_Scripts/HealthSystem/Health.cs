using System;
using _Scripts.HealthSystem;
using Unity.VisualScripting;
using UnityEngine;

public class Health : DamageableResources, IRecoverable
{
    public GameObject healTextPrefab;

    public event Action<RecoverResultData> OnRecover;
    
    // Health.cs
    private void Start()
    {
        LSO_Editor.Instance?.Register(this);
    }

    public void Recover(RecoverData data)
    {
        if(IsDestroyed) return;

        int recoverValue = data.recoverValue;
        Health giver = data.giver;

        int lastValue = Value;
        Value += recoverValue;
        int calcValue = Value;
        bool hasRecover = lastValue < calcValue;

        GameObject textObj = Instantiate(healTextPrefab, transform.position + Vector3.up, Quaternion.identity);
        textObj.GetComponent<KDH_HealAnim>().Setup(recoverValue);

        if (hasRecover)
        {
            Debug.Log($"{recoverValue}??? ???? ???????. ??????? : {calcValue}");
            RecoverResultData resultData = RecoverResultData.Create(giver, recoverValue, Value);
            OnRecover?.Invoke(resultData);
        }


    }
    
    //??? ??
    [ContextMenu("Recover")]
    public void Recover()
    {
        RecoverData data = RecoverData.Create(null, 1);
        if (IsDestroyed) return;

        int recoverValue = data.recoverValue;

        int lastValue = Value;
        Value += recoverValue;
        int calcValue = Value;
        bool hasRecover = lastValue < calcValue;

        if (hasRecover)
        {
            RecoverResultData resultData = RecoverResultData.Create(data.giver, recoverValue, calcValue);
            OnRecover?.Invoke(resultData);
        }
    }
    
}