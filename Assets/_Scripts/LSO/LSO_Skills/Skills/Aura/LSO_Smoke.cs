using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LSO_Smoke : MonoBehaviour,LSO_ISkill
{
    [SerializeField]private AudioClip clip;
    
    private LSO_PlayerMovement _playerMovement;
    private LSO_PlayerAttack _attack;
    private GameObject _player;
    private Rigidbody2D _rigid;
    private bool _canUse = true;
    
    [SerializeField]private GameObject effect;
    private GameObject _effectInstance;
    private Animator _animator;
    
    [SerializeField]private float coolTime = 5f;
    [SerializeField] private float waitTime = 0.5f;
    [SerializeField]private int damage = 10;
    [SerializeField ]private float speed = 10f;

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        
       _player = player;
       _playerMovement = _player.GetComponent<LSO_PlayerMovement>();
       _attack = _player.GetComponent<LSO_PlayerAttack>();
       _rigid = _player.GetComponent<Rigidbody2D>();
       
       LSO_SoundManager.Instance.SfxPlay(clip);
       player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(coolTime));
       //StartCoroutine(CoolTime(coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        
        //LSO_SoundManager.Instance.SfxPlay(clip);
        
        _effectInstance = Instantiate(effect, _player.transform.position, Quaternion.identity);
        _animator = _effectInstance.GetComponent<Animator>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_effectInstance.transform.position, _effectInstance.transform.localScale.x/2);
        foreach (Collider2D collision in colliders)
        {
            if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health enemyHealth))
            {
                DamageData data = DamageData.Create(enemyHealth, damage);
                enemyHealth.GetDamage(data);
            }
        }
        
        
        yield return  new WaitForSeconds(0.1f);
  
        _playerMovement.SetMove(false);
        //_rigid.linearVelocity = _playerMovement.GetFixedLastDir() * speed;
        _rigid.DOMove(_player.transform.position + _playerMovement.GetFixedLastDir() * speed, waitTime).SetEase(Ease.OutCubic);
        
        yield return new WaitForSeconds(waitTime);
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
        if (stateInfo.IsName("Animation-magic-5-g") && stateInfo.normalizedTime >= 0.95f)
        {
            Destroy(_effectInstance);
        }
    }
}
