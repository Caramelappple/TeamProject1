using System.Collections;
using UnityEngine;
public class LSO_PlayerAttack : MonoBehaviour
{
    [SerializeField] protected GameObject sword;
    private LSO_PlayerMovement _movement;
    private SpriteRenderer _sprite;
        
    private bool _attackable = true;
    private readonly float _attackCooldown = 0.12f;
    private readonly float _attackTime = 0.3f;//건드려도 됨
    private readonly int _damage = 10;//맘대로
    private bool _isAttacking;
    
    private Vector3 _lastDir;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponent<LSO_PlayerMovement>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        sword.SetActive(false);
    }
    public void OnAttack()
    {
        if (!_attackable) return;
    
        _lastDir = _movement.GetFixedLastDir();
        
        sword.transform.position = transform.position + _lastDir;
        StartCoroutine(Attack());
    }
    
    IEnumerator Attack()
    {
        _isAttacking = true;
        _attackable = false;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(sword.transform.position, sword.transform.localScale / 2, 0);
        foreach (Collider2D collision in colliders)
        {
            if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health enemyHealth))
            {
                Health health = gameObject.GetComponent<Health>();
                DamageData data = DamageData.Create(health, _damage);
                enemyHealth.GetDamage(data);
            }
        }

        sword.SetActive(true);
        _animator.SetTrigger("Attack");
        StartCoroutine(IsDirSame());

        float elapsed = 0f;
        while (elapsed < _attackTime)
        {
            if (!_isAttacking) // 방향 바뀌어서 취소됨
            {
                CancelAnim(_movement.GetFixedLastDir());
                sword.SetActive(false);

                // 취소 시점부터 쿨타임 새로 카운트
                yield return new WaitForSeconds(_attackCooldown+_attackTime);
                _attackable = true;
                // _isAttacking은 이미 false
                yield break; // ← 코루틴 완전 종료
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 정상 종료 경로
        sword.SetActive(false);
        yield return new WaitForSeconds(_attackCooldown);
        _attackable = true;
        _isAttacking = false;
    }

    public void SetAttack(bool attackable)
    {
        _attackable = attackable;
    }

    private IEnumerator IsDirSame()
    {
        while (true)
        {
            yield return null;
            if (_lastDir != _movement.GetFixedLastDir())
            {
                _isAttacking = false;
                yield break;
            }
        }
    }

    private void CancelAnim(Vector3 dir)
    {
        if (dir.normalized == Vector3.up)
        {
            _animator.Play("Up", 0, 0f);
        }else if (dir.normalized == Vector3.down)
        {
            _animator.Play("Down", 0, 0f);
        }
        else if (dir.normalized == Vector3.left || dir.normalized == Vector3.right)
        {
            _animator.Play("Next", 0, 0f);
            if (dir.x != 0)
            {
                _sprite.flipX = dir.x < 0;
            }
        }
        else
            Debug.Log(dir+"가 이상함 다시 코드 짜 플레이어 어택");
    }
}
