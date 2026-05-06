using System.Collections;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;

namespace _Scripts.NKY._EnemyScript
{
    public class NKY_Enemy : BaseBoss
    {
        [Header("???? ???? ???")]
        [SerializeField] private BossSkill[] _skills;
        public NKY_Player playerReference;

        private NKY_Health _myHealth;

        protected override void OnAwake()
        {
            _target = playerReference.gameObject;

            if (_skills != null)
            {
                foreach (var skill in _skills)
                {
                    skill.Init(this);
                }
            }
        }

        private void Start()
        {
            _myHealth = gameObject.GetComponent<NKY_Health>();
            if (_myHealth != null)
            {
                _myHealth.OnHit += IsHit;
                _myHealth.OnDamage += SetDamage;
            }

            StartCoroutine(BossMainRoutine());
        }

        protected override IEnumerator BossMainRoutine()
        {
            while (!_isDead)
            {
                yield return ExecutePattern(CentorMove());

                if(_isDead) yield break;

                yield return new WaitUntil(ShouldInterruptIdle);

                if (_isDead) yield break;

                IEnumerator nextSkill = PickNextSkill();
                yield return ExecutePattern(nextSkill);

                _lastSkillTime = Time.time;
            }
        }

        protected override IEnumerator PickNextSkill()
        {
            BossSkill selectedSkill = _skills[1];
            float randomSkill = Random.Range(0f, 100f);
            Debug.Log(randomSkill);
            if(randomSkill > 50)
                selectedSkill = _skills[0];
            else if(randomSkill <= 50)
                selectedSkill = _skills[1];

            return selectedSkill.Execute(transform, _target.transform);
        }

        //????? ?????? ????
        private void IsHit(NKY_DamageData data) //Enemy?? ?????? ?¨ú?????
        {
            Debug.Log($"hit to {data.giver.gameObject}");
        }
        private void SetDamage(NKY_DamageResultData args) //Enemy?? ???????? ???? ????? ???????
        {
            if (_isDead) return;
            

            if (_myHealth.IsDestroyed)
            {
                Die();
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Die()
        {
            _isDead = true;

            StopAllCoroutines();
            StopPattern();
            Destroy(_shadow.gameObject);
            
            if (_anim) _anim.Play("Dead");

            var col = GetComponent<Collider2D>();
            if (col) col.enabled = false;
        }
    }
}
