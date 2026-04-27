using Assets._Scripts.NKY;
using NKY.Player;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace NKY_Enemy
{
    public class NKY_Enemy : BaseBoss
    {
        [Header("보스 장착 스킬")]
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

                yield return new WaitUntil(() => ShouldInterruptIdle());

                if (_isDead) yield break;

                IEnumerator nextSkill = PickNextSkill();
                yield return ExecutePattern(nextSkill);

                _lastSkillTime = Time.time;
            }
        }

        protected override IEnumerator PickNextSkill()
        {
            float roll = Random.Range(0f, 100f);

            BossSkill selectedSkill = _skills[0];

            return selectedSkill.Execute(transform, _target.transform);
        }

        //공격시 발동시킬 이벤트
        public void IsHit(NKY_DamageData data) //Enemy의 공격이 맞았을때
        {
            Debug.Log($"hit to {data.giver.gameObject}");
        }
        public void SetDamage(NKY_DamageResultData args) //Enemy의 공격으로 인해 체력이 닳았을때
        {
            if (_isDead) return;

            int damage = args.damage;
            int currentHealth = args.currentHealth;
            Debug.Log($"{damage}정도 피달았고 {currentHealth}만큼 피 남음");

            if (_myHealth.IsDestroyed)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("보스 사망!!");
            _isDead = true;

            // 1. 진행 중이던 모든 스킬 코루틴과 메인 루틴 강제 정지
            StopAllCoroutines();
            StopPattern(); // PatternCoroutine에 만들어둔 안전 정지 메서드

            // 2. 사망 애니메이션 재생 (Animator에 "Die" 파라미터나 상태가 있다고 가정)
            if (_anim != null)
            {
                _anim.SetTrigger("Die");
                // 만약 트리거가 없고 특정 애니메이션을 직접 튼다면 _anim.Play("DieAnimName");
            }

            // 3. 충돌체(Collider) 끄기 - 죽은 시체에 플레이어가 막히거나 계속 때리는 것 방지
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            // 4. (옵션) 그림자 끄기 등 필요한 사후 처리 추가
        }
    }
}
