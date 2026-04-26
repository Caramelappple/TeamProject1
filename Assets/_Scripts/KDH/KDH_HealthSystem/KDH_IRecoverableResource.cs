//인터페이스를 상속할 때는 무족권 구현을 해야함.
using KDH_HealthSystem;
using System;
using static KDH_HealthSystem.KDH_RecoverData;

public interface KDH_IRecoverableResource
{
    public event Action<KDH_RecoverResultData> OnRecover;
    public void Recover(KDH_RecoverData recoverValue);
}