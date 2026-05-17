using System.Collections;
using UnityEngine;
using DG.Tweening;
public class LSO_PlayerAttack : MonoBehaviour
{
    [SerializeField] protected GameObject swordAxis;
    [SerializeField] protected GameObject sword;
    private LSO_PlayerMovement _movement;
    
    private bool _attackable = true;
    private readonly float _cooldown = 0.12f;//진짜 0.001 딘위정도는 건드려도 됨
    private readonly float _attackTime = 0.22f;//건드려도 됨
    private  readonly float _attackTime2 = 0.08f;//건드리면 절대 안됨
    private readonly int _damage = 10;//맘대로
    
    private Vector3 _lastDir;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponent<LSO_PlayerMovement>();
    }

    private void Start()
    {
        sword.SetActive(false);
    }
    public void OnAttack()
    {
        if (!_attackable) return;
    
        _lastDir = _movement.GetLastDir();
        
        sword.transform.position = transform.position + _lastDir;
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        _attackable = false;
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
        
        _movement.SetMove(false);
        _animator.SetTrigger("Attack");//애니메이션 재생
        transform.DOMove(transform.position-_lastDir * 0.2f, 0.15f);
        sword.SetActive(true);
        yield return new WaitForSeconds(_attackTime2);
        _movement.SetMove(true);
        yield return new WaitForSeconds(_attackTime);//공격 유지 시간 대기
        
       
        sword.SetActive(false);
        yield return new WaitForSeconds(_cooldown);//쿨타임 대기
        _attackable = true;
    }

    public void SetAttack(bool attackable)
    {
        _attackable = attackable;
    }
}
