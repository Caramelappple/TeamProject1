using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class LSO_PlayerAttack : PlayerMovement
{
    [SerializeField] private GameObject _swordAxis;
    [SerializeField] private GameObject _sword;
    private bool _attackable = true;
    private float _cooldown = 0.4f;
    private float _attackime = 0.2f;

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
        _sword.SetActive(true);
        SetRotation(_swordAxis, _lastDir);
        yield return new WaitForSeconds(_attackime);
        _sword.SetActive(false);
        yield return new WaitForSeconds(_cooldown);
        _attackable = true;
    }

    private void SetRotation(GameObject hitbox, Vector2 dir1)
    {
        float angle = Mathf.Atan2(dir1.y, dir1.x) * Mathf.Rad2Deg;
        //if (Math.Abs(angle) % 90 != 0)
        //    angle += 45 * Mathf.Sign(angle);
        hitbox.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        Debug.Log(angle);
    }
}
