using KSY.HealthSystem;
using System;
public interface KSY_IRecoverable
{
    public event Action<KSY_RecoverResultData> OnRecover;
    public void Recover(KSY_RecoverData recover);
}
