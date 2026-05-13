using UnityEngine;
using System.Collections.Generic;

public class LSO_BearTrap : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private int damage = 20;
    
    // 자기 자신의 LinkedList 노드를 직접 보유
    public LinkedListNode<GameObject> node;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health enemyHealth) 
            && collision.gameObject.CompareTag("Enemy"))
        {
            DamageData data = new DamageData(enemyHealth, damage);
            enemyHealth.GetDamage(data);
            _animator.SetTrigger("Explode");
        }
    }

    private void FixedUpdate()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("BearTrapExplode") && stateInfo.normalizedTime >= 0.95f)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        var spawner = LSO_BearTrapSpawner.instance;

        // 자기 자신의 노드를 정확히 제거
        if (node != null && node.List != null)
        {
            spawner.activeTraps.Remove(node);
            node = null;
        }

        gameObject.SetActive(false);
        spawner.trapPool.Push(gameObject);
    }
}