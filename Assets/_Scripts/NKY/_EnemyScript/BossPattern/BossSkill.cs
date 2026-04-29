using System.Collections;
using UnityEngine;

namespace _Scripts.NKY._EnemyScript.BossPattern
{
    public abstract class BossSkill : NKY_PatternCoroutine
    {
        protected BaseBoss _bossBrain;
        
        public virtual void Init(BaseBoss boss)
        {
            _bossBrain = boss;
            _anim = boss.GetComponent<Animator>();
            _shadow = boss.GetComponentInChildren<NKY_ShadowController>();
            _hitBoxController = boss.GetComponent<HitBoxController>();
        }
        public abstract IEnumerator Execute(Transform boss, Transform target);
    }
}
