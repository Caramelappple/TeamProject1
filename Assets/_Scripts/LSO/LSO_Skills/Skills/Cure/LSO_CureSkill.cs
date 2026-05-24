using System.Collections;
using _Scripts.HealthSystem;
using UnityEngine;

public class LSO_CureSkill : MonoBehaviour,LSO_ISkill
{
    private LSO_PlayerMovement _playerMovement;
    private Animator _animator;
    [SerializeField]private GameObject effect;
    private GameObject _effectInstance;
    private GameObject _player;
    
    [SerializeField] private float _coolTime = 10f;
    private float _waitTime = 2.6f;
    [SerializeField] private int _healValue = 20;
    
    private bool _canUse = true;
    
    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        Vector3 offset = new Vector3(0f, 0.4f, 0f);
        
        _player = player;
        _playerMovement = player.GetComponent<LSO_PlayerMovement>();
        _effectInstance = Instantiate(effect, player.transform.position+offset, Quaternion.identity);
        _effectInstance.transform.SetParent(player.transform);
        _animator = _effectInstance.GetComponent<Animator>();
        
        NKY_SoundManager.Instance.PlaySFX("CureExplode");
            
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        
        yield return new WaitForSeconds(_waitTime);

        if (_player.TryGetComponent<Health>(out Health health))
        {
            RecoverData data = new RecoverData(health, _healValue);
            health.Recover(data);
        }

        yield return new WaitForSeconds(time);
        _canUse = true;
    }
    
    private void FixedUpdate()
    {
        if (!_animator || !_effectInstance) return;
        
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Animation-magic-4-a") && stateInfo.normalizedTime >= 0.95f)
        {
            Destroy(_effectInstance);
        }
    }
}
