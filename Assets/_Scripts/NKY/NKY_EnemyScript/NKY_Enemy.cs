using System.Collections;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;

namespace _Scripts.NKY._EnemyScript
{
    public class NKY_Enemy: NKY_BaseBoss
    {
        
        [Header("보스의 스킬 패턴 세팅")]
        [SerializeField] private  NKY_BossSkill[] _skills;
        public LSO_PlayerMovement playerReference;

        [Header("보스 스텟 세팅")]
        [field: SerializeField] public int damage { get; private set; }

        private Health _myHealth;

        protected override void OnAwake()
        {
            _HitBoxController = GetComponent<NKY_HitBoxController>();
            _anim = GetComponent<Animator>();
            _shadow = GetComponent<NKY_ShadowController>();
            _myHealth = gameObject.GetComponent<Health>();
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
                if(bossPhase < 3)
                    yield return ExecutePattern(CentorMove());

                if (_isDead) yield break;
                
                yield return new WaitUntil(ShouldInterruptIdle);

                if (_isDead) yield break;

                IEnumerator nextSkill = PickNextSkill();
                yield return ExecutePattern(nextSkill);

                _lastSkillTime = Time.time;
            }
        }

        protected override IEnumerator PickNextSkill()
        {
            NKY_BossSkill selectedSkill = _skills[0];
            switch (bossPhase)
            {
                case 1:
                    selectedSkill = _skills[Random.Range(0, _skills.Length / 3)];
                    break;
                case 2:
                    selectedSkill = _skills[Random.Range(_skills.Length / 3, (_skills.Length * 2) / 3)];
                    break;
                case 3:
                    selectedSkill = _skills[Random.Range((_skills.Length * 2) / 3, _skills.Length)];
                    break;
                case 4:
                    selectedSkill = _skills[Random.Range(0, _skills.Length)];
                    break;
            }

            return selectedSkill.Execute(transform, _target.transform);
        }
        
        
        public void ReceiveAnimationAttackEvent()
        {
            _HitBoxController?.ResetHit();
            if (_attackEventQueue.Count > 0)
            {
                _attackEventQueue.Dequeue()?.Invoke();
            }
        }

        private IEnumerator PlayPhase2()
        {
            if(bossPhase == 2) yield break;
            
            Debug.Log("페이즈 2");
            bossPhase = 2;
            _skillCooldown *=  0.6f;
            damage = (int)(damage * 2f);
            
            yield return Phase2Effect();
            yield return ExecutePattern(CentorMove());
        }

        private IEnumerator Phase2Effect()
        {
            yield break;
        }

        private IEnumerator PlayPhase3()
        {
            if(bossPhase == 3) yield break;
            
            _myHealth.Value = _myHealth.MaxValue;
            
            Debug.Log("페이즈 3");
            bossPhase = 3;
            _skillCooldown *=  0.5f;
            damage = (int)(damage * 1.5f);
            
            yield return Phase3Effect();
            yield return ExecutePattern(CentorMove());
        }

        private IEnumerator Phase3Effect()
        {
            yield break;
        }

        //????? ?????? ????
        private void IsHit(DamageData data) //Enemy?? ?????? ?¾?????
        {
            
        }
        private void SetDamage(DamageResultData args) //Enemy?? ???????? ???? ????? ???????
        {
            if (_isDead) return;

            //2페이즈 세팅
            if (args.currentHealth < _myHealth.MaxValue / 2)
            {
                StartCoroutine(PlayPhase2());
            }

            if (_myHealth.IsDestroyed)
            {
                Die();
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Die()
        {
            if (bossPhase < 3)
            {
                StartCoroutine(PlayPhase3());
                return;
            }
            _isDead = true;

            StopAllCoroutines();
            StopPattern();

            if (_anim) _anim.Play("Dead");

            var col = GetComponent<Collider2D>();
            if (col) col.enabled = false;
        }
    }
}
