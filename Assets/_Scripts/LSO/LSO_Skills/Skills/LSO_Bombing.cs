using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class LSO_Bombing : LSO_PlayerMovement,LSO_ISkill
{
    private LSO_PlayerMovement _playerMovement;
    [SerializeField] private GameObject effect;
    private GameObject effectInstance;
    
    private Animator _animator;
    private GameObject _player;

    private void Start()
    {
        
    }

    public void UseSkill(GameObject player)
    {
        _player = player;

        if (!effectInstance)
        {
            effectInstance = Instantiate(effect, transform.position, transform.rotation);
            _animator = effect.GetComponent<Animator>();
        }
        else
        {
            effectInstance.transform.position = player.transform.position;
            effectInstance.SetActive(true);
        }
        
        AnimatorStateInfo _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (_stateInfo.IsName("Bombing") && _stateInfo.normalizedTime >= 0.95f)//애니메이션이 끝나면 트랩 비활성화
        {
            effectInstance.SetActive(false);
        }
    }

    public IEnumerator CoolTime(float time)
    {
        throw new NotImplementedException();
    }
}
