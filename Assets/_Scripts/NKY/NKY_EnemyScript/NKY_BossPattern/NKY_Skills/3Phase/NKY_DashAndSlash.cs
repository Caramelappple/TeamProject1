using System.Collections;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;

public class NKY_DashAndSlash : NKY_BossSkill
{
    [SerializeField] private float dashSpeed = 3f;
    [field: SerializeField] public override float DamageScale { get; protected set; } = 1f;
    public override IEnumerator Execute(Transform boss, Transform target)
    {
        Vector3 dir = target.position - boss.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        yield return PlaySequence(ShowWarn(0, new Vector2(2.5f, (target.position - boss.transform.position).magnitude + 2f), 4f, 
            () => Vector3.Lerp(boss.position, target.position, 0.5f), angle),
            ConstantMoveTo(boss, target.position, dashSpeed),
            WaitUntilOrTime(() => false, 0.8f)
            );
    }
}
