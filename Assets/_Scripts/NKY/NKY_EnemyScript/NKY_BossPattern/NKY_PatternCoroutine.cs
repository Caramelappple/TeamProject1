using System.Collections;
using System.Collections.Generic;
using _Scripts.NKY.Manager;
using UnityEngine;

namespace _Scripts.NKY._EnemyScript.BossPattern
{
    public abstract class NKY_PatternCoroutine : MonoBehaviour
    {
        private static readonly int Vanish = Animator.StringToHash("Vanish");
        private static readonly int Appear = Animator.StringToHash("Appear");

        //private static readonly int Vanish = Animator.StringToHash("Vanish");
        protected NKY_HitBoxController _HitBoxController;
        protected Animator _anim;
        protected NKY_ShadowController _shadow;

        private Coroutine _masterHandle;
        private Queue<System.Action> _attackEventQueue = new Queue<System.Action>();
        protected void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake() { }

        // ???? ????? (??? ???? ????)
        public Coroutine ExecutePattern(IEnumerator pattern)
        {
            if (_masterHandle != null) return null;
            _masterHandle = StartCoroutine(PatternMonitor(pattern));
            return _masterHandle;
        }

        private IEnumerator PatternMonitor(IEnumerator pattern)
        {
            yield return StartCoroutine(pattern);
            _masterHandle = null;
            _attackEventQueue.Clear();
        }

        public void StopPattern()
        {
            if (_masterHandle != null)
            {
                StopCoroutine(_masterHandle);
                _masterHandle = null;
            }
            _attackEventQueue.Clear();
            _anim.Play("Idle");
        }

        // --- ??????? ?? ???? ?????? ---
        public void OnAttackEvent()
        {
            NKY_BossSkill bossSkill = GetComponentInChildren<NKY_BossSkill>();
            _HitBoxController?.ResetHit();
            if (bossSkill._attackEventQueue.Count > 0)
            {
                bossSkill._attackEventQueue.Dequeue()?.Invoke();
            }
        }

        protected IEnumerator ComboAttack(string animName, params System.Action[] attackLogics)
        {
            _attackEventQueue.Clear();
            _HitBoxController?.ResetHit();
            foreach (var logic in attackLogics) 
                _attackEventQueue.Enqueue(logic);
            _anim.Play(animName);
            yield return StartCoroutine(WaitAnim(animName, 1.0f));
        }

        protected IEnumerator Attack(System.Action Logic)
        {
            _attackEventQueue.Clear();
            _HitBoxController?.ResetHit();
            _attackEventQueue.Enqueue(Logic);
            OnAttackEvent();
            yield break;
        }
        
        public IEnumerator DoShake(GameObject target, float duration, float power)
        {
            Vector3 originPos;
            
            originPos = target.transform.localPosition;

            float timer = 0;

            while (timer < duration)
            {
                Vector3 randomPos = Random.insideUnitSphere * power;

                target.transform.localPosition = originPos + randomPos;

                timer += Time.deltaTime;

                yield return new WaitForSeconds(Time.deltaTime);
            }

            target.transform.localPosition = originPos;
        }

        // --- ??? ?? ???? ?????? ---

        protected IEnumerator CentorMove()
        {
            yield return StartCoroutine(MoveTo(transform, new Vector2(0, 0), 1));
        }

        protected IEnumerator MoveTo(Transform from, Vector2 to, float duration)
        {
            float time = 0;
            Vector3 start = from.position;
            Vector3 end = to;
            while (time < duration)
            {
                from.position = Vector3.Lerp(start, end, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            from.position = end;
        }

        protected IEnumerator Move(Transform target, Vector2 direction, float distance, float duration)
        {
            Vector2 end = (Vector2)target.position + (direction.normalized * distance);
            yield return StartCoroutine(MoveTo(target, end, duration));
        }

        protected IEnumerator Teleport(Transform from, Vector2 to)
        {
            _anim.SetTrigger(Vanish);
            yield return StartCoroutine(WaitAnim("Vanish", 1f));
            from.position = to;
            _anim.SetTrigger(Appear);
            yield return StartCoroutine(WaitAnim("Appear", 0.6f));
        }

        // --- ??? ?????? ---
        protected IEnumerator WaitUntilOrTime(System.Func<bool> condition, float maxTime)
        {
            float t = 0;
            while (t < maxTime)
            {
                if (condition()) yield break;
                t += Time.deltaTime;
                yield return null;
            }
        }

        protected IEnumerator WaitAnim(string stateName, float normalizedTime)
        {
            yield return new WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).IsName(stateName));
            yield return new WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= normalizedTime);
        }

        protected IEnumerator PlaySequence(params IEnumerator[] skills)
        {
            foreach (var skill in skills) yield return StartCoroutine(skill);
        }

        protected IEnumerator MultiSequence(params IEnumerator[] skills)
        {
            foreach (var skill in skills) StartCoroutine(skill);
            yield break;
        }

        protected IEnumerator ShadowLock(bool isLocked)
        {
            if (isLocked) _shadow?.LockShadow();
            else _shadow?.UnlockShadow();
            yield break;
        }

        protected IEnumerator ShadowMoveLock(Vector3 position)
        {
            _shadow?.MoveToLock(position);
            yield break;
        }

        protected IEnumerator ShowWarn(Collider2D hitBox, float duration, System.Func<Vector2> positionGetter)
        {
            NKY_IndicatorManager.Instance.ShowIndicator(hitBox, positionGetter(), duration);
            yield break;
        }

        protected void HitToDamage(Collider2D target, int damage)
        {
            DamageData data = DamageData.Create(this.GetComponentInParent<Health>(), damage);
            if (target.TryGetComponent<Health>(out var health))
            {
                health.GetDamage(data);
            }
        }
    }
}
