//인터페이스를 상속할 때는 무족권 구현을 해야함.
using System;

public interface KDH_IRecoverableResource
{
    public event Action<KDH_RecoverEventArgs> OnRecover;
    public void Recover(int recoverValue); 
}