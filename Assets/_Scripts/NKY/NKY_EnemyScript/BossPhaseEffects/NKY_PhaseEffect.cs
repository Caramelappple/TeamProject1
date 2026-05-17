using System.Collections;
using _Scripts.NKY._EnemyScript;
using _Scripts.NKY._EnemyScript.BossPattern;

public abstract class NKY_PhaseEffect : NKY_PatternCoroutine
{
    protected NKY_Enemy _bossBrain;
    protected override void OnAwake()
    {
        _bossBrain = GetComponentInParent<NKY_Enemy>();
    }
    public abstract IEnumerator PlayPhaseEffect();
    
    public abstract IEnumerator EndPhaseEffect();
}
