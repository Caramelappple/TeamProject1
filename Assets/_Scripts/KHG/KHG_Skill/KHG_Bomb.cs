using System;
using System.Collections;
using UnityEngine;

public class KHG_Bomb : MonoBehaviour, LSO_ISkill
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float spawnDistance = 1.5f;
    [SerializeField] private float moveSpeed = 10f;

    private float _coolTime = 3f;
    private bool _canUse = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseSkill(gameObject);
        }
    }

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;

        LSO_PlayerMovement movement = player.GetComponent<LSO_PlayerMovement>();

        Vector2 dir = movement.GetFixedLastDir();

        if (dir == Vector2.zero)
            return;

        Vector3 spawnPos = player.transform.position +
                           (Vector3)(dir.normalized * spawnDistance);

        // 1. 여기서 생성된 폭탄의 이름(변수명)이 'poison'입니다.
        GameObject poison = Instantiate(
            bombPrefab,
            spawnPos,
            Quaternion.identity
        );

        Rigidbody2D rb = poison.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = dir.normalized * moveSpeed;
        }

        Destroy(poison, 5f);

        StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;

        yield return new WaitForSeconds(time);

        _canUse = true;
    }
}