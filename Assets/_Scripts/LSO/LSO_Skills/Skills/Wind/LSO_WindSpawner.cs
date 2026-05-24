using System.Collections;
using UnityEngine;

public class LSO_WindSpawner : MonoBehaviour,LSO_ISkill
{
    
    [SerializeField] private GameObject effect;
    private GameObject _effectInstance;
    [SerializeField] private float _coolTime = 10f;
    private LSO_PlayerMovement _playerMovement;
    private Rigidbody2D _rigid;
    private bool _canUse = true;

    private IEnumerator SkillCoolTime()
    {
        _canUse = false;
        yield return new WaitForSeconds(_coolTime);
        _canUse = true;
    }

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        _canUse = false;
        
        NKY_SoundManager.Instance.PlaySFX("Wind");        
        _effectInstance = Instantiate(effect, player.transform.position, Quaternion.identity);
        _effectInstance.GetComponent<LSO_Wind>().Init(player.GetComponent<Health>());
        _effectInstance.transform.SetParent(transform);
        _rigid = _effectInstance.GetComponent<Rigidbody2D>();
        _playerMovement = player.GetComponent<LSO_PlayerMovement>();
        
        _rigid.linearVelocity = _playerMovement.GetFixedLastDir();
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}
