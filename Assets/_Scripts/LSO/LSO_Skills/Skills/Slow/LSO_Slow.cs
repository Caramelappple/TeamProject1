using System.Collections;
using UnityEngine;

public class LSO_Slow : MonoBehaviour,LSO_ISkill
{
    [SerializeField]private GameObject effect;
    private GameObject _effectInstance;
    private Animator _animator;
    
    private bool _canUse = true;
    private float _coolTime = 60f;
    private float _waitTime = 5f;
    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        _effectInstance = Instantiate(effect, player.transform.position, Quaternion.identity);
        _animator = _effectInstance.GetComponent<Animator>();
        Time.timeScale = 0.5f;
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse  = false;
        yield return new WaitForSecondsRealtime(_waitTime);
        Time.timeScale = 1f;
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
}
