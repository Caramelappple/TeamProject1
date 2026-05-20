using System;
using System.Collections;
using _Scripts.HealthSystem;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.NKY._EnemyScript
{
    public class NKY_Enemy: NKY_BaseBoss
    {
        
        [Header("보스의 스킬 패턴 세팅")]
        [SerializeField] private  NKY_BossSkill[] skills;
        public LSO_PlayerMovement playerReference;

        [Header("보스 페이즈 효과")] 
        [SerializeField] private NKY_PhaseEffect phase2Effect;
        [SerializeField] private NKY_PhaseEffect phase3Effect;

        [Header("보스 스텟 세팅")]
        [field: SerializeField] public int Damage { get; private set; }

        private Health _myHealth;

        private IEnumerator _nextSkill;

        protected override void OnAwake()
        {
            _HitBoxController = GetComponent<NKY_HitBoxController>();
            Anim = GetComponent<Animator>();
            _shadow = GetComponent<NKY_ShadowController>();
            _myHealth = gameObject.GetComponent<Health>();
            _target = playerReference.gameObject;

            if (skills != null)
            {
                foreach (var skill in skills)
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
                
                if (_isDead || _isStunned) yield break;
                
                _nextSkill = PickNextSkill();
                yield return ExecutePattern(_nextSkill);
                
                _lastSkillTime = Time.time;
            }
        }

        protected override IEnumerator PickNextSkill()
        {
            NKY_BossSkill selectedSkill = skills[0];
            switch (bossPhase)
            {
                case 1:
                    selectedSkill = skills[Random.Range(0, skills.Length / 3)];
                    break;
                case 2:
                    selectedSkill = skills[Random.Range(skills.Length / 3, (skills.Length * 2) / 3)];
                    break;
                case 3:
                    selectedSkill = skills[Random.Range((skills.Length * 2) / 3, skills.Length)];
                    break;
                case 4:
                    selectedSkill = skills[Random.Range(0, skills.Length)];
                    break;
            }

            _bossSkill = selectedSkill;
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

        private void PlayPhase2()
        {
            if(bossPhase == 2 || phase2Effect == null) return;
            _isStunned = true;
            bossPhase = 2;
            _skillCooldown *=  0.6f;
            Damage = (int)(Damage * 2f);
            StopAllCoroutines();
            StartCoroutine(Phase2ProcessRoutine());
        }

        private IEnumerator Phase2ProcessRoutine()
        {
            if(_bossSkill != null)
                _bossSkill.EndSkill();
            _masterHandle = null;
            StartCoroutine(ShadowLock(false));
            yield return StartCoroutine(Phase2Effect());
            yield return StartCoroutine(CentorMove());
            _isStunned = false;
            StartCoroutine(BossMainRoutine());
        }

        private IEnumerator Phase2Effect()
        {
            yield return phase2Effect.PlayPhaseEffect();
        }

        private void PlayPhase3()
        {
            if(bossPhase == 3 || phase3Effect == null) return;
            
            _isStunned = true;
            if (_isDead)
            {
                _isDead = false;
                _myHealth.Value += 1;
                RecoverData data = RecoverData.Create(_myHealth, _myHealth.MaxValue);
                _myHealth.Recover(data);
            }
            Anim.Play("Idle");
            bossPhase = 3;
            _skillCooldown *=  0.5f;
            Damage = (int)(Damage * 1.5f);
            
            StopAllCoroutines();
            StartCoroutine(Phase3ProcessRoutine());
        }

        private IEnumerator Phase3ProcessRoutine()
        {
            if(_bossSkill != null)
                _bossSkill.EndSkill();
            _masterHandle = null;
            StartCoroutine(ShadowLock(false));
            yield return StartCoroutine(Phase3Effect());
            yield return StartCoroutine(CentorMove());
            _isStunned = false;
            StartCoroutine(BossMainRoutine());
        }

        private IEnumerator Phase3Effect()
        {
            yield return phase2Effect.EndPhaseEffect();
            yield return phase3Effect.PlayPhaseEffect();
        }

        //????? ?????? ????
        private void IsHit(DamageData data) //Enemy?? ?????? ?¾?????
        {
            
        }
        private void SetDamage(DamageResultData args) //Enemy?? ???????? ???? ????? ???????
        {
            //2페이즈 세팅
            if (args.currentHealth < _myHealth.MaxValue / 2 && bossPhase < 2)
            {
                PlayPhase2();
            }
            
            Debug.Log(bossPhase);
            if (_myHealth.IsDestroyed)
            {
                _isDead = true;
                Die();
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Die()
        {
            if (bossPhase < 3)
            {
                PlayPhase3();
                return;
            }
            
            if(_bossSkill != null)
                _bossSkill.EndSkill();
            StopAllCoroutines();
            StopPattern();
            StartCoroutine(phase3Effect.EndPhaseEffect());
            if (Anim) Anim.Play("Dead");

            var col = GetComponent<Collider2D>();
            if (col) col.enabled = false;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Health health))
            {
                DamageData data = DamageData.Create(_myHealth, Damage);
                health.GetDamage(data);
            }
        }
    }
}
