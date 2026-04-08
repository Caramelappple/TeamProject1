using System;
using System.Diagnostics.Tracing;

public class KSY_Health : KSY_DamageableResource, KSY_IRecoverableResource
{
    public event Action<KSY_RecoverEventArgs> OnRecover;
    public KSY_RecoverEventArgs eventArgs = new KSY_RecoverEventArgs();
    private void Awake()
    {
        Initialize(10,0,5);
    }
    public void Recover(int recoverValue)
    {
        int lastValue = Value;
        Value += recoverValue;

        eventArgs.giver = gameObject;
        eventArgs.recoverValue = recoverValue;
        eventArgs.resourceValue = Value;
        
        OnRecover?.Invoke(eventArgs);
    }
}
