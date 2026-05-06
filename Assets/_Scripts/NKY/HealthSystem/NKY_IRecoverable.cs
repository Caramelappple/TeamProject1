using System;
using UnityEngine;

public interface NKY_IRecoverable
{
    public event Action<NKY_RecoverResultData> OnRecovery;
    public void Recover(NKY_RecoverData recoverValue);
}
