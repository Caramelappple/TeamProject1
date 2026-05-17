using System.Collections;
using UnityEngine;

namespace _Scripts.NKY._EnemyScript.BossPattern
{
    public abstract class NKY_BossSkill : NKY_PatternCoroutine
    {
        protected NKY_Enemy _bossBrain;

        protected override void OnAwake()
        {
            _bossBrain = GetComponentInParent<NKY_Enemy>();
        }

        public abstract float DamageScale { get; protected set; }
        
        public virtual void Init(NKY_BaseBoss boss)
        {
            Anim = boss.GetComponent<Animator>();
            _shadow = boss.GetComponentInChildren<NKY_ShadowController>();
            _HitBoxController = boss.GetComponent<NKY_HitBoxController>();
        }
        public abstract IEnumerator Execute(Transform boss, Transform target);
        
        protected IEnumerator ComboAttack(string animName, params System.Action[] attackLogics)
        {
            _bossBrain._attackEventQueue.Clear();
            _HitBoxController?.ResetHit();
            foreach (var logic in attackLogics) 
                _bossBrain._attackEventQueue.Enqueue(logic);
            Anim.Play(animName);
            yield return StartCoroutine(WaitAnim(animName, 1.0f));
            _bossBrain._attackEventQueue.Clear();
        }

        protected IEnumerator Attack(System.Action Logic)
        {
            _bossBrain._attackEventQueue.Clear();
            _HitBoxController?.ResetHit();
            _bossBrain._attackEventQueue.Enqueue(Logic);
            _bossBrain.ReceiveAnimationAttackEvent();
            yield break;
        }
    }
}
