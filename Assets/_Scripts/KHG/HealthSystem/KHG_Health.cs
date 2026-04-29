using KHG.HealthSystme;
using System;


public class KHG_Health : KHG_DamageableResource
{
    public KHG_RecoverData eventArgs = new KHG_RecoverData();
    public event Action<KHG_RecoverResultData> OnRecover;

    public void Recover(KHG_RecoverData data)
    {
        if (isDestroyed)
        {
            return;
        }

        int recoverValue = data.recoverValue;
        KHG_Entity giver = data.giver;

        int lastValue = Value;
        Value += recoverValue;
        int calcValue = Value;
        bool hasRecover = lastValue < calcValue;

        if (hasRecover)
        {
            KHG_RecoverResultData resultData = KHG_RecoverResultData.Create(giver, recoverValue, calcValue);
            OnRecover?.Invoke(resultData);
        }
    }
}
