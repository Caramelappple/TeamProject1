using System.Collections;
using UnityEngine;

public class LSO_FireAura : MonoBehaviour,LSO_ISkill
{
    private LSO_PlayerMovement _playerMovement;
    private GameObject _player;
    private bool _canUse = true;
    
    [SerializeField]private GameObject effect;
    private GameObject _effectInstance;
    private SpriteRenderer _sprite;
    
    private float _coolTime = 5f;
    private float _waitTime = 0.2f;

    private int _damage = 10;
    private float speedMult = 5f;
    
    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        
       _player = player;
       _playerMovement = _player.GetComponent<LSO_PlayerMovement>();
       
       player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        
        _effectInstance = Instantiate(effect, _player.transform.position, Quaternion.identity);
        _sprite = _effectInstance.GetComponent<SpriteRenderer>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_effectInstance.transform.position, _effectInstance.transform.localScale.x/2);
        foreach (Collider2D collision in colliders)
        {
            if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health enemyHealth))
            {
                DamageData data = DamageData.Create(enemyHealth, _damage);
                enemyHealth.GetDamage(data);
            }
        }
        float originalSpeed = _playerMovement.speed;
        _playerMovement.speed *= speedMult;
        Debug.Log(_playerMovement.speed);
        
        yield return  new WaitForSeconds(0.1f);
        _sprite.sprite = null;    
        
        yield return new WaitForSeconds(_waitTime);
        Destroy(_effectInstance);
        _playerMovement.speed = originalSpeed;
        
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}
