using Assets._Scripts.NKY;
using NKY.Player;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NKY_Enemy
{
    public class NKY_Enemy : PatternCoroutine
    {
        private List<System.Func<IEnumerator>> patterns;
        [SerializeField] private Collider2D[] hitboxes;
        //[SerializeField] private int damege = 1;

        public NKY_Player target;

        private bool isMove = true;
        Vector3 moveDir;
        float moveSpeed = 3;
        private void Awake()
        {
            _hitBoxController = GetComponent<HitBoxController>();
            _anim = GetComponent<Animator>();
            _target = target;

            patterns = new List<System.Func<IEnumerator>>()
            {
                Skill1
            };
        }
        private void Start()
        {
            var hp = target.GetComponent<NKY_DamageableResources>();

            if (hp != null)
            {
                hp.OnHit += IsHit;
                hp.OnDamage += SetDamage;
            }
            StartCoroutine(MainRoutine());
        }
        //private void Update()
        //{
        //    EnemyMove();
        //}

        private IEnumerator MainRoutine()
        {
            yield return StartCoroutine(Skill1()); 
        }


        //공격시 발동시킬 이벤트
        public void IsHit(NKY_DamageData data) //Enemy의 공격이 맞았을때
        {
            Debug.Log($"hit to {data.giver.gameObject}");
        }
        public void SetDamage(NKY_DamageResultData args) //Enemy의 공격으로 인해 체력이 닳았을때
        {
            int damage = args.damage;
            int currentHealth = args.currentHealth;
            Debug.Log($"{damage}정도 피달았고 {currentHealth}만큼 피 남음");
        }

        //이동 메서드
        //private void EnemyMove()
        //{
        //    if (!isMove)
        //        return;
        //    moveDir.x = (target.transform.position.x - transform.position.x);
        //    moveDir.Normalize();
        //    if (moveDir.x < 0)
        //        transform.rotation = Quaternion.Euler(0, 180, 0);
        //    else
        //        transform.rotation = Quaternion.Euler(0, 0, 0);
        //    transform.position += moveDir * moveSpeed * Time.deltaTime;
        //}

        protected IEnumerator Teleport(Transform from, Transform to)
        {
            _anim.SetTrigger("Vanish");
            yield return StartCoroutine(WaitAnim("Vanish", 1));
            from.position = new Vector2(to.position.x, from.position.y);
            _anim.SetTrigger("Appear");
            yield return StartCoroutine(WaitAnim("Appear", 1));
            yield break;
        }
        //스킬 코루틴
        protected override IEnumerator Skill1()
        {
            return PlaySequence(
                Teleport(transform, target.transform),
                Move(transform, Vector2.up, 0.5f, 0.2f),
                WaitUntilOrTime(() => false, 0.3f),
                Move(transform, Vector2.down, 5.5f, 0.2f),
                AttackWithAnim(hitboxes[0], 4, "StationaryAttack"),
                WaitUntilOrTime(() => false, 1.3f),
                CentorMove()
                );
        }

        //protected override IEnumerator Skill2()
        //{

        //}

        private IEnumerator CentorMove()
        {
            yield return StartCoroutine(MoveTo(transform, new Vector2(0, 0), 1));
        }
    }
}
