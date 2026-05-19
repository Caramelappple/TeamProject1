using System.Collections;
using _Scripts.NKY._EnemyScript;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;

public abstract class NKY_PhaseEffect : NKY_PatternCoroutine
{
    protected NKY_Enemy _bossBrain;
    protected Animator _bossAnimator;
    protected override void OnAwake()
    {
        _bossBrain = GetComponentInParent<NKY_Enemy>();
        _bossAnimator = _bossBrain.GetComponent<Animator>();
    }
    public abstract IEnumerator PlayPhaseEffect();
    
    public abstract IEnumerator EndPhaseEffect();
}
