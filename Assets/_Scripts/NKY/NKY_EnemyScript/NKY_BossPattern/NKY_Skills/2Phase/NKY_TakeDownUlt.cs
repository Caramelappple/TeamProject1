using System;
using System.Collections;
using _Scripts.NKY._EnemyScript;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;

namespace _Scripts.NKY.NKY_EnemyScript.NKY_Skills
{
    public class NKY_TakeDownUlt : NKY_BossSkill
    {
        private static readonly int Vanish = Animator.StringToHash("Vanish");
        private static readonly int Appear = Animator.StringToHash("Appear");

        [Header("보스 스킬 세팅")]
        [SerializeField] private Collider2D[] slamHitbox;
        [SerializeField] private GameObject dustEffect;

        [field: SerializeField] public override float damageScale { get; protected set; } = 0.5f;

        private int _damage = 0;

        private void Start()
        {
            _damage = (int)(damageScale * _bossBrain.GetComponent<NKY_Enemy>().damage);
            dustEffect.SetActive(false);
        }

        public override IEnumerator Execute(Transform boss, Transform target)
        {
            Vector3 targetPos;
            for (int i = 0; i < 3; i++)
            {
                _anim.SetTrigger(Vanish);
                yield return StartCoroutine(WaitAnim("Vanish", 1f));
                
                targetPos = target.position;
                boss.position = targetPos + new Vector3(0, 3, 0);
                yield return ShadowMoveLock(targetPos);

                _anim.SetTrigger(Appear);
                yield return StartCoroutine(WaitAnim("Appear", 0.6f));
                
                yield return PlaySequence(
                    ShowWarn(slamHitbox[0], 0.8f, () => _shadow.transform.position),
                    ShowWarn(slamHitbox[2], 0.8f, () => _shadow.transform.position));

                yield return PlaySequence(
                    Move(boss, Vector2.up, 0.5f, 0.1f),
                    WaitUntilOrTime(() => false, 0.15f),
                    MoveTo(boss, targetPos, 0.2f),
                    ComboAttack("StationaryAttack",
                        () => _HitBoxController.Cast(slamHitbox[2], (hitTarget) => HitToDamage(hitTarget, (int)_damage)),
                        () => _HitBoxController.Cast(slamHitbox[0], (hitTarget) => HitToDamage(hitTarget, (int)_damage)),
                        () => _HitBoxController.Cast(slamHitbox[1], (hitTarget) => HitToDamage(hitTarget, (int)_damage))
                    ),
                    ShowWarn(slamHitbox[3], 0.6f, () => _shadow.transform.position),
                    WaitUntilOrTime(() => false, 0.4f),
                    Attack(() => _HitBoxController.Cast(slamHitbox[3], (hitTarget) => HitToDamage(hitTarget, (int)_damage * 2))),
                    PlayDustEffect(dustEffect, _shadow.transform.position),
                    ShadowLock(false)
                );
                Init();
            }
            yield break;
        }

        private IEnumerator PlayDustEffect(GameObject effect, Vector2 pos)
        {
            Animator effectAnim =  effect.GetComponent<Animator>();
            effect.SetActive(true);
            effectAnim.Play("PlayDustEffect");
            yield return WaitAnim(effectAnim, "PlayDustEffect", 0.9f);
        }

        private void Init()
        {
            dustEffect.SetActive(false);
        }
    }
}
