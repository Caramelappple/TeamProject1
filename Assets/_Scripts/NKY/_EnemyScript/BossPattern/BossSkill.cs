using System.Collections;
using UnityEngine;

public abstract class BossSkill : NKY_PatternCoroutine
{
    // 보스의 뇌(상태)를 연결해둘 변수
    protected BaseBoss _bossBrain;

    // 보스가 게임 시작 시점에 이 함수를 호출해서 뇌를 동기화 시켜줄 겁니다.
    public virtual void Init(BaseBoss boss)
    {
        _bossBrain = boss;
        _anim = boss.GetComponent<Animator>();
        _shadow = boss.GetComponentInChildren<NKY_ShadowController>();
        _hitBoxController = boss.GetComponent<HitBoxController>();
    }
    public abstract IEnumerator Execute(Transform boss, Transform target);
}
