using System;
using System.Collections;
using UnityEngine;

public class LSO_FireAura : MonoBehaviour,LSO_ISkill
{
    private LSO_PlayerMovement _playerMovement;
    private LSO_PlayerAttack _attack;
    private GameObject _player;
    private Rigidbody2D _rigid;
    private bool _canUse = true;
    
    [SerializeField]private GameObject effect;
    private GameObject _effectInstance;
    private Animator _animator;
    
    private float _coolTime = 5f;
    private float _waitTime = 0.5f;
    private int _damage = 10;
    private float _speed = 10f;

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        
       _player = player;
       _playerMovement = _player.GetComponent<LSO_PlayerMovement>();
       _attack = _player.GetComponent<LSO_PlayerAttack>();
       _rigid = _player.GetComponent<Rigidbody2D>();
       
       player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        
        _effectInstance = Instantiate(effect, _player.transform.position, Quaternion.identity);
        _animator = _effectInstance.GetComponent<Animator>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_effectInstance.transform.position, _effectInstance.transform.localScale.x/2);
        foreach (Collider2D collision in colliders)
        {
            if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health enemyHealth))
            {
                DamageData data = DamageData.Create(enemyHealth, _damage);
                enemyHealth.GetDamage(data);
            }
        }
        
        
        yield return  new WaitForSeconds(0.1f);
  
        _playerMovement.SetMove(false);
        _rigid.linearVelocity = _playerMovement.GetLastDir() * _speed;
        
        yield return new WaitForSeconds(_waitTime);
        _attack.OnAttack();
        _rigid.linearVelocity = Vector2.zero;
        _playerMovement.SetMove(true);
        
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
    
    private void FixedUpdate()
    {
        if (!_animator || !_effectInstance) return;
        
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Animation-magic-2-b") && stateInfo.normalizedTime >= 0.95f)
        {
            Destroy(_effectInstance);
        }
    }
}
