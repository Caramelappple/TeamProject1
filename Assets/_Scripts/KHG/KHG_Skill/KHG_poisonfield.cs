using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class KHG_PoisonField : MonoBehaviour,LSO_ISkill
{
    [SerializeField] private GameObject poisonPrefab;
    [SerializeField] private float spawnDistance = 1.5f;
    private float _coolTime = 3f;
    private LSO_PlayerMovement _playerMovement;
    private bool _canUse = true;
    
    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        
        _playerMovement = player.GetComponent<LSO_PlayerMovement>();
        
        Vector2 lookDirection = _playerMovement.GetLastDir();

        Vector3 spawnPos = transform.position + new Vector3(lookDirection.x, lookDirection.y, 0) * spawnDistance;

        GameObject poison = Instantiate(poisonPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = poison.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = lookDirection.normalized * 5f; // 발사 속도
        }

        Destroy(poison, 5f);
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }
    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}