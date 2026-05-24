using UnityEngine;
using System.Collections;

public class LSO_EnergyBallSpawner : MonoBehaviour,LSO_ISkill
{
    [SerializeField] private AudioClip clip;
    
    [SerializeField] private GameObject effect;
    private GameObject _effectInstance;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _coolTime = 30f;
    [SerializeField] private float _recoilTime = 0.3f;
    [SerializeField] private float _recoilSpeed = 1.8f;
    private Vector2 _lookDirection;
    private LSO_PlayerMovement _playerMovement;
    private Rigidbody2D _rigid;
    private bool _canUse = true;

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        
        LSO_SoundManager.Instance.SfxPlay(clip);
        _playerMovement = player.GetComponent<LSO_PlayerMovement>();
        _lookDirection = _playerMovement.GetFixedLastDir();
        _effectInstance = Instantiate(effect, player.transform.position, Quaternion.identity);
        _rigid = player.GetComponent<Rigidbody2D>();
        
        Rigidbody2D rigid = _effectInstance.GetComponent<Rigidbody2D>(); 
        rigid.linearVelocity = _lookDirection.normalized * _speed; // 발사 속도
        //rigid.DOMove(_effectInstance.transform.position+(Vector3)_lookDirection * _speed, 2.8f).SetEase(Ease.OutSine);
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
        
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        
        _playerMovement.SetMove(false);
        _rigid.linearVelocity = _lookDirection.normalized * -_recoilSpeed;
        yield return new WaitForSeconds(_recoilTime);
        _playerMovement.SetMove(true);
        _rigid.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}

