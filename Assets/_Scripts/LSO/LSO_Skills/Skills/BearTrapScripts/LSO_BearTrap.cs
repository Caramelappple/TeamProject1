using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LSO_BearTrap : MonoBehaviour, LSO_ISkill
{
    [SerializeField] private AudioClip clip;   
    
    private Animator _animator;
    [SerializeField] private int damage = 20;
    
    // 자기 자신의 LinkedList 노드를 직접 보유
    public LinkedListNode<GameObject> node;

    private bool _isReturning = false; // 중복 실행 방지

    private void OnEnable()
    {
        _isReturning = false; // 풀에서 꺼낼 때 초기화
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // _animator가 null이거나 아직 반환 중이면 실행하지 않음
        if (_animator == null || _isReturning) return; 
        
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
    
        if (stateInfo.IsName("BearTrapExplode") && stateInfo.normalizedTime >= 0.95f)
        {
            _isReturning = true;
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        var spawner = LSO_BearTrapSpawner.instance;
        
        if (node != null && node.List == spawner.activeTraps)
        {
            spawner.activeTraps.Remove(node);
            node = null;
        }

        gameObject.SetActive(false);
        spawner.trapPool.Push(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isReturning && collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<Health>(out Health enemyHealth))
            {
                LSO_SoundManager.Instance.SfxPlay(clip);
                // 데미지 처리
                DamageData data = new DamageData(enemyHealth, damage);
                enemyHealth.GetDamage(data);
                _animator.SetTrigger("Explode");
            }
        }
    }

    public void UseSkill(GameObject player)
    {
        Debug.Log("함정 설치");
    }

    public IEnumerator CoolTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
}