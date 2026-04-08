using System;
public interface KSY_IRecoverableResource
{
    public event Action<KSY_RecoverEventArgs> OnRecover;
    public void Recover(int recoverValue);
}
