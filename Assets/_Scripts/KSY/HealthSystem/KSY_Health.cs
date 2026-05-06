using Assets._Scripts.KSY;
using KSY.HealthSystem;
using System;
using UnityEngine;
public class KSY_Health : KSY_DamageableResource, KSY_IRecoverable
{
    public event Action<KSY_RecoverResultData> OnRecover;

    private void Awake()
    {
        base.Awake();
        OnRecover += (data) => Debug.Log($"<color=green>{data.giver}로부터 {data.recoverValue}만큼 힐을 받았습니다!</color>");
    }
    // <회복 구현>
    // - 회복 되었을 때 회복 이벤트 호출.
    // - 죽었을 때 회복되면 안됩니다.

    public void Recover(KSY_RecoverData data)
    {
        //죽었다면 회복이 되면 안됨.
        if (IsDestroyed) return;

        //지역 변수 초기화.
        int recoverValue = data.recoverValue;
        Assets._Scripts.KSY.KSY_Entity giver = data.giver;

        //회복이 되었는지 확인하는 로직
        int lastValue = Value;
        //회복하는 로직
        Value += recoverValue;
        int calcValue = Value;
        bool hasRecover = lastValue < calcValue;

        //만약 회복이 되었다면, 회복이 된 것을 구독자들에게 알리기.
        if(hasRecover)
        {
            KSY_RecoverResultData resultData = KSY_RecoverResultData.Create(giver, recoverValue,calcValue);
            OnRecover?.Invoke(resultData);
        }
    }

    [ContextMenu("Recover")]
    public void Recover()
    {
        KSY_RecoverData data = KSY_RecoverData.Create(null, 1);
        if (IsDestroyed) return;

        int recoverValue = data.recoverValue;

        int lastValue = Value;
        Value += recoverValue;
        int calcValue = Value;
        bool hasRecover = lastValue < calcValue;

        if (hasRecover)
        {
            KSY_RecoverResultData resultData = KSY_RecoverResultData.Create(data.giver, recoverValue, calcValue);
            OnRecover?.Invoke(resultData);
        }
    }
}
