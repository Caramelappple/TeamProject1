using System;
using _Scripts.HealthSystem;
using UnityEngine;

public interface NKY_IRecoverable
{
    public event Action<NKY_RecoverResultData> OnRecover;
    public void Recover(NKY_RecoverData recoverValue);
}
