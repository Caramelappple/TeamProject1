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
        SetRotation(_swordAxis, _lastDir);
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        _sword.SetActive(true);

        yield return new WaitForSeconds(_attackime);
        _sword.SetActive(false);

        yield return new WaitForSeconds(_cooldown);
        _attackable = true;
    }
    public void SetRotation(GameObject hitbox, Vector2 dir1)
    {
        float angle = Mathf.Atan2(dir1.y, dir1.x) * Mathf.Rad2Deg;
        hitbox.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        Debug.Log(angle);
    }

}
