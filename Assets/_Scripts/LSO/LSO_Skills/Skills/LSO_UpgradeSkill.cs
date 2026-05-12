using System.Collections;
using UnityEngine;

public class LSO_UpgradeSkill : MonoBehaviour,LSO_ISkill
{
    [SerializeField] GameObject effect;
    private GameObject _effectInstance;
    private GameObject _player;
    private LSO_PlayerMovement _playerMovement;
    
    private bool _canUse = true;
    private readonly float _coolTime = 0.15f;
    private readonly float _waitTime = 0.3f;
    private readonly int _damage = 10;
    
    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        
        _player = player;
        _playerMovement = player.GetComponent<LSO_PlayerMovement>();
        
        _effectInstance = Instantiate(effect, player.transform);
        _effectInstance.transform.parent = transform;
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
            }
        }
        
        yield return new WaitForSeconds(_waitTime);
        Destroy(_effectInstance);

        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}
