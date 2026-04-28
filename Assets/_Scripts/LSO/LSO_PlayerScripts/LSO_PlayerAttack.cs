using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class LSO_PlayerAttack : PlayerMovement
{
    [SerializeField] protected GameObject _swordAxis;
    [SerializeField] protected GameObject _sword;
    private bool _attackable = true;
    private float _cooldown = 0.2f;
    private float _attackime = 0.3f;

    private void Start()
    {
        _sword.SetActive(false);
    }
    private void OnAttack()
    {
        if (!_attackable) return;
        IEnumerator attack = Attack();
        StartCoroutine(attack);
    }

    IEnumerator Attack()
    {
        _attackable = false;
        _sword.transform.position = transform.position+(Vector3)_lastDir;//공격 히트박스 이동
        _animator.SetTrigger("Attack");//애니메이션 재생
        yield return new WaitForSeconds(0.1f);
        _sword.SetActive(true);

        yield return new WaitForSeconds(_attackime);//공격 유지 시간 대기
        _sword.SetActive(false);

        yield return new WaitForSeconds(_cooldown);//쿨타임 대기
        _attackable = true;
    }
}
