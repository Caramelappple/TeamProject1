using UnityEngine;
using System.Collections;

public class LSO_EnergyBall : MonoBehaviour,LSO_ISkill
{
    [SerializeField] private GameObject effect;
    private GameObject _effectInstance;
    [SerializeField] private float spawnDistance = 1;
    private float _speed = 12;
    private float _coolTime = 3f;
    private float _waitTime = 5f;
    private LSO_PlayerMovement _playerMovement;
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
        
        _playerMovement = player.GetComponent<LSO_PlayerMovement>();
        Vector2 lookDirection = _playerMovement.GetLastDir();
        _effectInstance = Instantiate(effect, player.transform.position + (Vector3)lookDirection * spawnDistance, Quaternion.identity);

        
        Rigidbody2D rigid = _effectInstance.GetComponent<Rigidbody2D>();
        rigid.linearVelocity = lookDirection.normalized * _speed; // 발사 속도
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
        
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        yield return new WaitForSeconds(_waitTime);
        Destroy(_effectInstance);
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}

