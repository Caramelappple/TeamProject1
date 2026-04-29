
using KHG.HealthSystme;
using System;


public interface KHG_IRecoverableResource
{
    public event Action<KHG_RecoverResultData>  Onmove;
    public void Recover(KHG_RecoverData recover);
}
