using NKY.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public abstract class PatternCoroutine : MonoBehaviour
{
    protected HitBoxController _hitBoxController;
    protected NKY_Player _target;
    protected Animator _anim;
    protected Collider2D _currentHitBox;
    protected int _currentDamage;

    protected IEnumerator AttackWithAnim(Collider2D hitBox, int damage, string animName)
    {
        _currentHitBox = hitBox;
        _currentDamage = damage;

        _anim.SetTrigger(animName);

        yield return StartCoroutine(WaitAnim(animName, 1));
    }
    public void Attack()
    {
        _hitBoxController.ResetHit();

        _hitBoxController.Cast(_currentHitBox, (target) =>
        {
            if (target.TryGetComponent<NKY_Health>(out var hp))
            {
                var data = NKY_DamageData.Create(hp, _currentDamage);
                hp.GetDamage(data);
            }
        });
    }
    protected IEnumerator ComboAttack(params IEnumerator[] actions)
    {
        foreach (var action in actions)
        {
            yield return StartCoroutine(action);
        }
    }
    protected IEnumerator MultiAttack(Collider2D[] hitBoxes, int damage, string animName)
    {
        _hitBoxController.ResetHit();
        foreach (var hitBox in hitBoxes)
        {
            AttackWithAnim(hitBox, damage, animName);
        }
        yield break;
    }


    protected IEnumerator WaitUntilOrTime(System.Func<bool> condition, float maxTime) 
    {
        float t = 0;

        while (t < maxTime)
        {
            if (condition())
                yield break;

            t += Time.deltaTime;
            yield return null;
        }
    }
    protected IEnumerator Move(Transform target, Vector2 direction, float distance, float duration)
    {
        float time = 0;

        Vector3 start = target.position;
        Vector3 end = start + (Vector3)(direction.normalized * distance); 
        if (duration <= 0)
        {
            target.position = end;
            yield break;
        }

        while (time < duration)
        {
            target.position = Vector3.Lerp(start, end, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        target.position = end;
    }
    protected IEnumerator MoveTo(Transform target, Vector2 to, float duration)
    {
        float time = 0;

        Vector3 start = target.position;
        Vector3 end = to;
        Vector2 dir = (start - end).normalized;
        if (duration <= 0)
        {
            target.position = end;
            yield break;
        }

        while (time < duration)
        {
            target.position = Vector3.Lerp(start, end, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        target.position = end;
    }


    protected IEnumerator PlayActionWithAnim(string animName, IEnumerator action)
    {
        _anim.SetTrigger(animName);

        yield return StartCoroutine(WaitAnim(animName, 0.4f));

        if (action != null)
            yield return StartCoroutine(action);

        yield return StartCoroutine(WaitAnim(animName, 1));
    }
    protected IEnumerator PlayAnim(string animName)
    {
        _anim.SetTrigger(animName);
        yield break;
    }
    protected IEnumerator WaitAnim(string stateName , float time)
    {
        yield return new WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).IsName(stateName));
        yield return new WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= time);
    }
    protected IEnumerator PlaySequence(params IEnumerator[] skills)
    {
        foreach (var skill in skills)
        {
            yield return StartCoroutine(skill);
        }
    }

    protected abstract IEnumerator Skill1();

    //protected abstract IEnumerator Skill2();
}
