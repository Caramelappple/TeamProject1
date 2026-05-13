using System.Collections;
using UnityEngine;

public class LSO_Bombing : MonoBehaviour,LSO_ISkill
{
    private static readonly int Explode = Animator.StringToHash("Explode");
    private Animator _animator;
    private LSO_PlayerMovement _playerMovement;
    private bool _canUse = true;

    [SerializeField] private GameObject effect;
    private GameObject _effectInstance;

    private readonly float _waitTime = 0.6f;
    [SerializeField] private float coolTime = 5f;

    [SerializeField] private int selfDamage = 30;
    [SerializeField] private int damage = 80;

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;

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

    public IEnumerator CoolTime(float coolTime)
    {
        _canUse = false;
        yield return new WaitForSeconds(_waitTime); // 애니메이션 끝날 때까지 대기

        // 폭발 위치 기준으로 범위 데미지
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_effectInstance.transform.position, 4);
        foreach (Collider2D collision in colliders)
        {
            if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out Health enemyHealth))
            {
                DamageData data = DamageData.Create(enemyHealth, damage);
                enemyHealth.GetDamage(data);
            }
        }

        _effectInstance.SetActive(false);
        yield return new WaitForSeconds(coolTime);//쿨타임 대기
        _canUse = true;
    }
}
