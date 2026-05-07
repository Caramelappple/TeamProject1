using System;
using System.Collections;
using UnityEngine;

public class LSO_Bombing : MonoBehaviour,LSO_ISkill
{
    private LSO_PlayerMovement _playerMovement;
    [SerializeField] private GameObject effect;
    private GameObject _effectInstance;
    private bool _canUse = true;
    private float _coolTime = 10f;
    private GameObject _player;
    
    private Animator _animator;

    public void UseSkill(GameObject player)
    {
        _player = player;
        
        if (!_canUse) return;
        
        if (!_effectInstance)
        {
            _effectInstance = Instantiate(effect, player.transform.position, transform.rotation);
            _animator = _effectInstance.GetComponent<Animator>();
        }
        else
        {
            _effectInstance.transform.position = player.transform.position;
            _effectInstance.SetActive(true);
        }

        _animator.SetTrigger("Explode");
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    private void FixedUpdate()
    {
        if (!_animator || !_effectInstance) return;

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Bombing") && stateInfo.normalizedTime >= 0.95f)
        {
            _effectInstance.SetActive(false);
        }
    }

    private void Update()
    {
        if (!_player || _canUse || _effectInstance) return;
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_effectInstance.transform.position,10, 0);
        foreach (Collider2D collision in collider2Ds)
        {
                Debug.Log(collision.name);
        }
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}
