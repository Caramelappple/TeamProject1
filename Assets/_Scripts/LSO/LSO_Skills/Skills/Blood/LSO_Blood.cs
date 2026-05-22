using System.Collections;
using _Scripts.HealthSystem;
using UnityEngine;

public class LSO_Blood : MonoBehaviour,LSO_ISkill
{
    [SerializeField] private AudioClip clip;
    
    private LSO_PlayerMovement _playerMovement;
    private GameObject _player;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool _canUse = true;
    private Vector3 _lastDir;
    
    private float _coolTime = 5f;
    private float _waitTime = 1f;
    private int _damage = 30;
    private int _healValue = 10;
    
    [SerializeField]private GameObject hEffect;
    [SerializeField]private GameObject vEffect;
    
    private GameObject _effectInstance;//이펙트 복제한것

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        
        
        _player = player;
        _playerMovement = player.GetComponent<LSO_PlayerMovement>();
        _lastDir = _playerMovement.GetFixedLastDir();
        
    
        //이펙트 가로 세로 정해주기
        if (_lastDir.x != 0)
        {
            _effectInstance = Instantiate(hEffect, player.transform.position, Quaternion.identity);
        }
        else if (_lastDir.y != 0)
        {
            _effectInstance = Instantiate(vEffect, player.transform.position, Quaternion.identity);
        }
        LSO_SoundManager.Instance.SfxPlay(clip);
        
        Vector2 dir = new  Vector2(_effectInstance.transform.position.x+_lastDir.x * 2.4f, _effectInstance.transform.position.y + _lastDir.y + 0.5f);
        
        //보는 방향 맞춰서 뒤집어 주기
        if (_effectInstance != null)
        {
            _spriteRenderer = _effectInstance.GetComponent<SpriteRenderer>();

            if (_lastDir.x != 0)
            {
                _spriteRenderer.flipX = _lastDir.x < 0;
            }
            
            if (_lastDir.y != 0)
            {
                _spriteRenderer.flipY = _lastDir.y < 0;
            }
            
            //보는 방향으로 히트박스 이동해주기
            _effectInstance.transform.position = dir;
            
            if (!_animator)
            {
                _animator = _effectInstance.GetComponent<Animator>();
            }
        }
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        
        //데미지 및 회복
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_effectInstance.transform.position, _effectInstance.transform.localScale/2, 0);
        foreach (Collider2D collision in colliders)
        {
            if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health enemyHealth))
            {
                //맞은 적에게 데미지
                DamageData data = DamageData.Create(enemyHealth, _damage);
                enemyHealth.GetDamage(data);
                
                //적이 맞을 때마다 회복
                _player.TryGetComponent<Health>(out Health playerHealth);
                RecoverData recover = RecoverData.Create(playerHealth, _healValue);
                playerHealth.Recover(recover);
            }
        }
        
        yield return new WaitForSeconds(_waitTime);
        Destroy(_effectInstance);

        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}
