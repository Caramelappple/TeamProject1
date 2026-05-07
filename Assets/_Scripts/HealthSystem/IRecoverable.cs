using System;
using _Scripts.HealthSystem;
using UnityEngine;

public interface IRecoverable
{
    public event Action<RecoverResultData> OnRecover;
    public void Recover(RecoverData recoverValue);
}
