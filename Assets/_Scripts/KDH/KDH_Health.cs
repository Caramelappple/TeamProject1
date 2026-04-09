using System;
using UnityEngine;

public class KDH_Health : KDH_DamageAbleResorce, KDH_IRecoverableResource
{
    public event Action<KDH_RecoverEventArgs> OnRecover;
    public KDH_RecoverEventArgs eventArgs = new KDH_RecoverEventArgs();

    private void Awake()
    {
        Initalize(10, 0, 5);
    }

    public void Recover(int recoverValue)
    {
        int lastValue = Value;
        Value += recoverValue;

        if (Value < lastValue)
        {
            eventArgs.giver = gameObject;
            eventArgs.recoverValue = recoverValue;
            eventArgs.resourceValue = Value;


            OnRecover?.Invoke(eventArgs);
        }
    }
}