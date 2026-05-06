using System.Collections;
using UnityEngine;

namespace _Scripts.NKY._EnemyScript.BossPattern
{
    public abstract class NKY_BossSkill : NKY_PatternCoroutine
    {
        protected NKY_BaseBoss _bossBrain;
        
        public virtual void Init(NKY_BaseBoss boss)
        {
            _bossBrain = boss;
            _anim = boss.GetComponent<Animator>();
            _shadow = boss.GetComponentInChildren<NKY_ShadowController>();
            nkyHitBoxController = boss.GetComponent<NKY_HitBoxController>();
        }
        public abstract IEnumerator Execute(Transform boss, Transform target);
        
        protected IEnumerator projectileAttackRoutine(GameObject projectile, Vector3 direction, float distance, float duration)
        {
            // PlaySequence가 끝날 때까지 이 코루틴 안에서만 대기합니다.
            // 메인 Execute 코루틴의 for문에는 영향을 주지 않습니다.
            yield return PlaySequence(
                WaitUntilOrTime(() => false, 0.6f),
                Move(projectile.transform, projectile.transform.up, distance, duration)
            );
        }
    }
}
