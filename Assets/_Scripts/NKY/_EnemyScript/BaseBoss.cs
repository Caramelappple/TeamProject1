using System.Collections;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;

namespace _Scripts.NKY._EnemyScript
{
    public abstract class BaseBoss : NKY_PatternCoroutine
    {
        protected GameObject _target;

        [Header("AI Settings")]
        [SerializeField] protected float _skillCooldown = 3.0f;
        protected float _lastSkillTime = -99f;

        protected bool _isDead = false;

        protected bool IsSkillReady()
        {
            return Time.time >= _lastSkillTime + _skillCooldown;
        }

        protected bool ShouldInterruptIdle()
        {
            float dist = Vector2.Distance(transform.position, _target.transform.position);
            if (dist < 1.5f) return true;

            return IsSkillReady();
        }

        protected abstract IEnumerator PickNextSkill();

        protected abstract IEnumerator BossMainRoutine();
    }
}
