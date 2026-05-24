using System.Collections;
using _Scripts.NKY._EnemyScript.BossPattern;
using _Scripts.NKY.Manager;
using UnityEngine;

namespace _Scripts.NKY.NKY_EnemyScript.NKY_Skills
{
    public class NKY_TakeDown : NKY_BossSkill
    {
        private static readonly int Vanish = Animator.StringToHash("Vanish");
        private static readonly int Appear = Animator.StringToHash("Appear");

        [Header("보스 스킬 세팅")]
        [SerializeField] private Collider2D[] slamHitbox;

        [field: SerializeField] public override float DamageScale { get; protected set; } = 0.5f;

        private Collider2D bossCol;

        //private int _damage;

        private void Start()
        {
            _damage = (int)(DamageScale * _bossBrain.Damage);
        }

        public override IEnumerator Execute(Transform boss, Transform target)
        {
            bossCol = boss.GetComponent<Collider2D>();
            
            Vector3 targetPos;
            for (int i = 0; i < 3; i++)
            {
                bossCol.isTrigger = true;
                Anim.SetTrigger(Vanish);
                yield return StartCoroutine(WaitAnim("Vanish", 1f));
                
                targetPos = target.position;
                boss.position = targetPos + new Vector3(0, 3, 0);
                yield return ShadowMoveLock(targetPos);

                Anim.SetTrigger(Appear);
                yield return StartCoroutine(WaitAnim("Appear", 0.6f));
                
                yield return PlaySequence(
                    ShowWarn((NKY_IndicatorManager.IndicatorType)1, new Vector2(12f, 2f), 0.8f, () => _shadow.transform.position),
                    ShowWarn(slamHitbox[2], 0.8f, () => _shadow.transform.position));

                yield return PlaySequence(
                    Move(boss, Vector2.up, 0.5f, 0.1f),
                    WaitUntilOrTime(() => false, 0.15f),
                    MoveTo(boss, targetPos, 0.2f),
                    WaitUntilOrTime(() => false, 0.2f),
                    ComboAttack("StationaryAttack",
                        () => _HitBoxController.Cast(slamHitbox[2], (hitTarget) => HitToDamage(boss.gameObject, hitTarget.gameObject, _damage)),
                        () => _HitBoxController.Cast(slamHitbox[0], (hitTarget) => HitToDamage(boss.gameObject, hitTarget.gameObject, _damage)),
                        () => _HitBoxController.Cast(slamHitbox[1], (hitTarget) => HitToDamage(boss.gameObject, hitTarget.gameObject, _damage))
                    ),
                    ShadowLock(false)
                );
                bossCol.isTrigger = false;
                yield return WaitUntilOrTime(() => false, 0.8f);
            }
            yield break;
        }

        public override void EndSkill()
        {
            Anim.Play("Idle");
            StartCoroutine(ShadowLock(false));
            bossCol.isTrigger = false;
        }
    }
}
