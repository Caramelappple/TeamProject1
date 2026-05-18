using System;
using System.Collections;
using UnityEngine;

public class LSO_Slow : MonoBehaviour,LSO_ISkill
{
    private float _tolerance = 0.1f;
    [SerializeField]private GameObject effect;
    private GameObject _effectInstance;
    private Animator _animator;
    private GameObject _player;
    
    private bool _canUse = true;
    private float _coolTime = 60f;
    private float _waitTime = 5f;
    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        _player = player;
        _effectInstance = Instantiate(effect, player.transform.position, Quaternion.identity);
        _animator = _effectInstance.GetComponent<Animator>();
        _player.GetComponent<MonoBehaviour>().StartCoroutine(SetScale(0.5f));
        player.GetComponent<MonoBehaviour>().StartCoroutine(SetSat(-100));
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse  = false;
        yield return new WaitForSecondsRealtime(_waitTime);
        _player.GetComponent<MonoBehaviour>().StartCoroutine(SetScale(1));
        _player.GetComponent<MonoBehaviour>().StartCoroutine(SetSat(0));
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
    
    private void FixedUpdate()
    {
        // _animator가 null이거나 아직 반환 중이면 실행하지 않음
        if (!_animator || !_effectInstance) return; 
        
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
    
        if (stateInfo.IsName("Animation-magic-12-a") && stateInfo.normalizedTime >= 0.95f)
        {
           Destroy(_effectInstance);
        }
    }
    
    private IEnumerator SetSat(float value)
    {
        while (Math.Abs(LSO_Editor.Instance.colorGrading.saturation.value - value) > _tolerance)
        {
            yield return null;
            LSO_Editor.Instance.colorGrading.saturation.value +=
                LSO_Editor.Instance.colorGrading.saturation.value > value ? -0.5f : 0.5f;
        }
    }
    
    private IEnumerator SetScale(float value)
    {
        while (Math.Abs(Time.timeScale - value) > _tolerance)
        {
            yield return null;
            Time.timeScale  +=
                Time.timeScale > value ? -0.5f : 0.5f;
        }
    }
}
