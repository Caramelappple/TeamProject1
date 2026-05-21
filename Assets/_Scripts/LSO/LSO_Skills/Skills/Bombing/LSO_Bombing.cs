using System.Collections;
using UnityEngine;

public class LSO_Bombing : MonoBehaviour,LSO_ISkill
{
    [SerializeField] private AudioClip clip;
    
    private static readonly int Explode = Animator.StringToHash("Explode");
    private Animator _animator;
    private LSO_PlayerMovement _playerMovement;
    private bool _canUse = true;

    [SerializeField] private GameObject effect;
    private GameObject _effectInstance;
    
    [SerializeField] private float coolTime = 5f;
    [SerializeField] private int selfDamage = 30;
    

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        
        LSO_SoundManager.Instance.SfxPlay(clip); // 오디오 실행
        if (!_effectInstance || !_animator)
        {
            _effectInstance = Instantiate(effect, player.transform.position, transform.rotation);
            _animator = _effectInstance.GetComponent<Animator>();
        }
        else
        {
            _effectInstance.transform.position = player.transform.position;
            _effectInstance.SetActive(true);
        }

        // 자기 자신 데미지
        if (player.TryGetComponent<Health>(out Health health))
        {
            DamageData data = DamageData.Create(health, selfDamage);
            health.GetDamage(data);
        }

        _animator.SetTrigger(Explode);
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        yield return new WaitForSeconds(time);//쿨타임 대기
        _canUse = true;
    }
}
