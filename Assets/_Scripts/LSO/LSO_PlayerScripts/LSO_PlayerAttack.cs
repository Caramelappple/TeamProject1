using System.Collections;
using UnityEngine;
//<<<<<<< HEAD
using DG.Tweening;

//=======
//using DG.Tweening;  
//>>>>>>> base
public class LSO_PlayerAttack : MonoBehaviour
{
    [SerializeField] protected GameObject swordAxis;
    [SerializeField] protected GameObject sword;
    private LSO_PlayerMovement _movement;
    private bool _attackable = true;
    private readonly float _cooldown = 0.12f;
    private readonly float _attackTime = 0.25f;
    private  readonly float _attackTime2 = 0.05f;
    private readonly int _damage = 10;
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
    private void OnAttack()
    {
        if (!_attackable) return;
    
        _lastDir = _movement.GetLastDir();
        
        sword.transform.position = transform.position + _lastDir;
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        _attackable = false;
        
        Vector3 targetDir = new Vector3(transform.position.x - _lastDir.x, transform.position.y - _lastDir.y, transform.position.z - _lastDir.z).normalized;
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
//<<<<<<< HEAD
//=======
        //transform.DOMove(targetDir * 0.001f, 0.05f);
//>>>>>>> base
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
