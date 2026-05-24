using System.Collections;
using _Scripts.HealthSystem;
using _Scripts.NKY._EnemyScript.BossPattern;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.NKY._EnemyScript
{
    public class NKY_Enemy : NKY_BaseBoss
    {
        [SerializeField] private NKY_BossIntro intro;
        [Header("������ ��ų ���� ����")]
        [SerializeField] private NKY_BossSkill[] skills;
        public LSO_PlayerMovement playerReference;
        [SerializeField] private NKY_BossPatternEnd bossPatternEnd = null;

        [Header("���� ������ ȿ��")]
        [SerializeField] private NKY_PhaseEffect phase2Effect;
        [SerializeField] private NKY_PhaseEffect phase3Effect;

        [Header("���� ���� ����")]
        [field: SerializeField] public int Damage { get; private set; }

        private Health _myHealth;

        private IEnumerator _nextSkill;

        protected override void OnAwake()
        {
            _HitBoxController = GetComponent<NKY_HitBoxController>();
            Anim = GetComponent<Animator>();
            if (Anim == null)
            {
                Anim = GetComponentInChildren<Animator>();
            }
            _shadow = GetComponent<NKY_ShadowController>();
            _myHealth = gameObject.GetComponent<Health>();
            //playerReference = NKY_GameManager.instance.player.GetComponent<LSO_PlayerMovement>();
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
            intro.gameObject.SetActive(false);
            if (_myHealth != null)
            {
                _myHealth.OnHit += IsHit;
                _myHealth.OnDamage += SetDamage;
            }
            StartCoroutine(PlayBoss());
        }

        private IEnumerator PlayBoss()
        {
            yield return StartCoroutine(intro.PlayIntro());
            StartCoroutine(BossMainRoutine());
        }

        protected override IEnumerator BossMainRoutine()
        {
            while (!_isDead)
            {
                if (bossPhase < 3)
                    yield return ExecutePattern(CentorMove());

                if (_isDead) yield break;

                yield return new WaitUntil(ShouldInterruptIdle);

                if (_isDead || _isStunned) yield break;

                _nextSkill = PickNextSkill();
                yield return ExecutePattern(_nextSkill);
                if (bossPatternEnd != null)
                    yield return bossPatternEnd.BossPatternEnd(transform);
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
            if (bossPhase == 2 || phase2Effect == null) return;
            _isStunned = true;
            bossPhase = 2;
            _skillCooldown *= 0.6f;
            Damage = (int)(Damage * 2f);
            StopAllCoroutines();
            StartCoroutine(Phase2ProcessRoutine());
        }

        private IEnumerator Phase2ProcessRoutine()
        {
            foreach (NKY_BossSkill skill in skills)
            {
                skill.ChangedDamage();
            }
            if (_bossSkill != null)
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
            if (bossPhase == 3 || phase3Effect == null) return;

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
            _skillCooldown *= 0.5f;
            Damage = (int)(Damage * 1.5f);

            StopAllCoroutines();
            StartCoroutine(Phase3ProcessRoutine());
        }

        private IEnumerator Phase3ProcessRoutine()
        {
            foreach (NKY_BossSkill skill in skills)
            {
                skill.ChangedDamage();
            }
            if (_bossSkill != null)
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
        private void IsHit(DamageData data) //Enemy?? ?????? ?��?????
        {

        }
        private void SetDamage(DamageResultData args) //Enemy?? ???????? ???? ????? ???????
        {
            //2������ ����
            if (phase2Effect != null || (args.currentHealth < _myHealth.MaxValue / 2 && bossPhase < 2))
            {
                PlayPhase2();
            }


            if (_myHealth.IsDestroyed)
            {
                _isDead = true;
                Die();
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Die()
        {
            if (phase3Effect != null || bossPhase < 3)
            {
                PlayPhase3();
                return;
            }

            if (_bossSkill != null)
                _bossSkill.EndSkill();
            StopAllCoroutines();
            StopPattern();
            if (phase3Effect != null)
                StartCoroutine(phase3Effect.EndPhaseEffect());
            Camera.main.DOShakePosition(0.5f, 2f);
            Anim.Play("Dead");

            var col = GetComponent<Collider2D>();
            if (col) col.enabled = false;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Health health))
            {
                DamageData data = DamageData.Create(_myHealth, (int)(Damage * 0.2f));
                health.GetDamage(data);
            }
        }
    }
}