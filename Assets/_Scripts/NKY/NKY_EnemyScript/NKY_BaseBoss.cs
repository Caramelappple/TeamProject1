using System.Collections;
using System.Collections.Generic;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;

namespace _Scripts.NKY._EnemyScript
{
    public abstract class NKY_BaseBoss : NKY_PatternCoroutine
    {
        protected GameObject _target;

        protected int bossPhase = 1;
        
        public Queue<System.Action> _attackEventQueue { get; private set; } = new Queue<System.Action>();
        

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
    }
}
