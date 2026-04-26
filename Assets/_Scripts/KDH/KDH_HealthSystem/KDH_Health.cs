using KDH_HealthSystem;
using NUnit.Framework.Internal;
using System;
using UnityEngine;
using UnityEngine.UI;
using static KDH_HealthSystem.KDH_RecoverData;
using static UnityEngine.Rendering.DebugUI;

public class KDH_Health : KDH_DamageAbleResorce, KDH_IRecoverableResource
{
    public event Action<KDH_RecoverResultData> OnRecover;
    public KDH_DamageData eventArgs = new KDH_DamageData();

    private KDH_CurrentHPText text;

    // <회복 구현> Health에서
    // - 회복되었을 때 회복 이벤트 호출
    // - 죽었을 때 회복되면 안됩니다.
    public void Recover(KDH_RecoverData data)
    {
        if (_isDestroyed) return;

        int recoverValue = data.recoverValue;
        KDH_Entity giver = data.giver;

        int lastValue = Value;
        //
        Value += recoverValue;
        //
        int calcValue = Value;
        bool hasRecover = lastValue < calcValue;

        if (hasRecover)
        {
            KDH_RecoverResultData resultData = KDH_RecoverResultData.Create(giver, recoverValue, calcValue);
            OnRecover?.Invoke(resultData);
        }
    }
}