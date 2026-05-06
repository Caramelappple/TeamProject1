using System.Collections;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;

namespace _Scripts.NKY.NKY_EnemyScript.NKY_Skills
{
    public class NKY_TakeDown : NKY_BossSkill
    {
        private static readonly int Vanish1 = Animator.StringToHash("Vanish");
        private static readonly int Appear = Animator.StringToHash("Appear");

        [Header("내려찍기 스킬 세팅")]
        [SerializeField] private Collider2D[] _slamHitbox;
        [SerializeField] private float _damage = 4f;

        public override IEnumerator Execute(Transform boss, Transform target)
        {
            Vector3 targetPos;
            for (int i = 0; i < 3; i++)
            {
                _anim.SetTrigger(Vanish1);
                yield return StartCoroutine(WaitAnim("Vanish", 1f));
                
                targetPos = target.position;
                boss.position = targetPos + new Vector3(0, 3, 0);
                yield return ShadowMoveLock(targetPos);

                _anim.SetTrigger(Appear);
                yield return StartCoroutine(WaitAnim("Appear", 0.6f));
                
                yield return PlaySequence(
                    ShowWarn(_slamHitbox[0], 0.8f, () => _shadow.transform.position),
                    ShowWarn(_slamHitbox[2], 0.8f, () => _shadow.transform.position));

                yield return PlaySequence(
                    Move(boss, Vector2.up, 0.5f, 0.1f),
                    WaitUntilOrTime(() => false, 0.15f),
                    MoveTo(boss, targetPos, 0.2f),
                    ComboAttack("StationaryAttack",
                        () =>
                        {
                            nkyHitBoxController.Cast(_slamHitbox[0], (target) => HitToDamage(target, (int)_damage));
                            nkyHitBoxController.Cast(_slamHitbox[2], (target) => HitToDamage(target, (int)_damage));
                        },
                        () => nkyHitBoxController.Cast(_slamHitbox[1], (target) => HitToDamage(target, (int)_damage))
                    ),
                    ShadowLock(false),
                    WaitUntilOrTime(() => false, 0.8f)
                );
            }
            yield break;
        }
    }
}
