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
    }
}
