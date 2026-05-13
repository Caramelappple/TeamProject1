using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class LSO_PlayerAttack : LSO_PlayerMovement
{
    [SerializeField] protected GameObject swordAxis;
    [SerializeField] protected GameObject sword;
    private bool _attackable = true;
    private readonly float _cooldown = 0.15f;
    private readonly float _attackime = 0.3f;
    private readonly int _damage = 10;
    private Vector3 _lastDir;

    private void Start()
    {
        sword.SetActive(false);
    }
    private void OnAttack()
    {
        if (!_attackable) return;
        IEnumerator attack = Attack();
        StartCoroutine(attack);
        
        _lastDir = gameObject.GetComponent<LSO_PlayerMovement>().GetLastDir();
    }

    IEnumerator Attack()
    {
        _attackable = false;
        sword.transform.position = transform.position + _lastDir;//공격 히트박스 이동
        
        Collider2D[] colliders = Physics2D.OverlapBoxAll(sword.transform.position, sword.transform.localScale/2, 0);
        foreach (Collider2D collision in colliders)
        {
            if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health enemyHealth))
            {
                Health health = gameObject.GetComponent<Health>();
                DamageData data = DamageData.Create(health, _damage);
                enemyHealth.GetDamage(data);
            }
        }
        
        Animator.SetTrigger("Attack");//애니메이션 재생
        sword.SetActive(true);
        
        yield return new WaitForSeconds(_attackime);//공격 유지 시간 대기
        sword.SetActive(false);
        yield return new WaitForSeconds(_cooldown);//쿨타임 대기
        _attackable = true;
    }

    public void SetAttack(bool attackable)
    {
        _attackable = attackable;
    }
    //sdafa
}
