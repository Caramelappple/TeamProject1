using System.Collections;
using UnityEngine;

public class TakeDown : BossSkill
{
    [Header("내려찍기 스킬 세팅")]
    // 보스 스크립트에 있던 인디케이터, 데미지, 공격 범위 변수들을 이쪽으로 가져옵니다!
    [SerializeField] private Collider2D[] _slamHitbox;
    [SerializeField] private float _damage = 4f;

    public override IEnumerator Execute(Transform boss, Transform target)
    {
        Vector3 targetPos;
        for (int i = 0; i < 3; i++)
        {
            _anim.SetTrigger("Vanish");
            yield return StartCoroutine(WaitAnim("Vanish", 1f));

            targetPos = target.position;

            boss.position = targetPos + new Vector3(0, 3, 0);
            yield return ShadowMoveLock(targetPos);

            _anim.SetTrigger("Appear");
            yield return StartCoroutine(WaitAnim("Appear", 0.6f));

            //yield return new WaitForSeconds(0.2f);

            yield return PlaySequence(
                ShowWarn(_slamHitbox[0], 0.8f, () => targetPos),
                Move(boss, Vector2.up, 0.5f, 0.1f),
                WaitUntilOrTime(() => false, 0.15f),
                MoveTo(boss, targetPos, 0.2f),
                ComboAttack("StationaryAttack",
                    () => _hitBoxController.Cast(_slamHitbox[0], (target) => HitToDamage(target, (int)_damage)),
                    () => _hitBoxController.Cast(_slamHitbox[1], (target) => HitToDamage(target, (int)_damage))
                ),
                ShadowLock(false),
                WaitUntilOrTime(() => false, 0.8f)
            );
        }
        yield break;
    }
}
