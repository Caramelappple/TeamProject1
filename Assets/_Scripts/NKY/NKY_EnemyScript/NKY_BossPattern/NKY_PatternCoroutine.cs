using System.Collections;
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
        public Animator Anim { get; protected set; }
        protected NKY_ShadowController _shadow;

        
        protected void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake() { }

        // ???? ????? (??? ???? ????)

        // --- ??????? ?? ???? ?????? ---
        
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

        protected IEnumerator ConstantMoveTo(Transform form, Vector3 to, float speed)
        {
            float currentTime = 0;
            Vector3 start = form.position;
            while (Vector3.Distance(form.position, to) > 0.1f)
            {
                form.position = Vector3.MoveTowards(start, to, currentTime);
            
                // 다음 프레임까지 대기
                currentTime += speed * Time.deltaTime;
                yield return null; 
            }
            
            form.position = to;
        }

        protected IEnumerator Move(Transform target, Vector2 direction, float distance, float duration)
        {
            Vector2 end = (Vector2)target.position + (direction.normalized * distance);
            yield return StartCoroutine(MoveTo(target, end, duration));
        }

        protected IEnumerator Teleport(Transform from, Vector2 to)
        {
            Anim.SetTrigger(Vanish);
            yield return StartCoroutine(WaitAnim("Vanish", 1f));
            from.position = to;
            Anim.SetTrigger(Appear);
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
            yield return new WaitUntil(() => Anim.GetCurrentAnimatorStateInfo(0).IsName(stateName));
            yield return new WaitUntil(() => Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= normalizedTime);
        }
        protected IEnumerator WaitAnim(Animator anim, string stateName, float normalizedTime)
        {
            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName(stateName));
            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= normalizedTime);
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
        protected IEnumerator ShowWarn(NKY_IndicatorManager.IndicatorType type, Vector2 size, float duration, System.Func<Vector2> positionGetter, float angle = 0f)
        {
            NKY_IndicatorManager.Instance.ShowIndicator(type, positionGetter(), size, duration,  angle);
            yield break;
        }

        protected void HitToDamage(GameObject giver, GameObject target, int damage)
        {
            if (target.TryGetComponent(out Health health))
            {
                Health healthGiver = giver.GetComponent<Health>();
                DamageData data = DamageData.Create(healthGiver, damage);
                health.GetDamage(data);
            }
        }
    }
}
