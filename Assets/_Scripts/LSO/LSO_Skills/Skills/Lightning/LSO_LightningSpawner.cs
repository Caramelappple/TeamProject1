using System.Collections;
using UnityEngine;

public class LSO_LightningSpawner : MonoBehaviour, LSO_ISkill
{
    [SerializeField] private AudioClip[] clips;

    private GameObject _player;
    private bool _canUse = true;
    private Collider2D[] _targets;
    private GameObject _target;

    private readonly float _diff = 100;

    [SerializeField] private float _coolTime = 5f;
    private float _minWaitTime = 0.1f;
    private float _maxWaitTime = 0.4f;
    private int _minCount = 4;
    private int _maxCount = 10;
    private float _range = 1.2f;
    private float _nearRange = 100;

    [SerializeField] private GameObject effect;
    private GameObject _effectInstance;

    private bool _isDestroyed;

    private void OnDestroy()
    {
        _isDestroyed = true;
    }

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        _player = player;
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        _target = GetNearest();

        if (_target == null)
        {
            Debug.LogWarning("No target found");
            yield break;
        }

        for (int i = 0; i < Random.Range(_minCount, _maxCount); i++)
        {
            if (_isDestroyed) yield break; //삭제됐으면 중단
            if (!_target) break;

            LSO_SoundManager.Instance.SfxPlay(clips[Random.Range(0, clips.Length)]);

            _effectInstance = Instantiate(
                effect,
                _target.transform.position + new Vector3(
                    Random.Range(-_range, _range),
                    Random.Range(-_range, _range)),
                Quaternion.identity
            );
            
            _effectInstance.transform.parent = transform;

            yield return new WaitForSeconds(Random.Range(_minWaitTime, _maxWaitTime));
        }

        yield return new WaitForSeconds(time);
        
        if (!_isDestroyed)
            _canUse = true;
    }

    private GameObject GetNearest()
    {
        _targets = Physics2D.OverlapCircleAll(_player.transform.position, _nearRange);

        GameObject result = null;
        float diff = this._diff;

        foreach (Collider2D target in _targets)
        {
            if (!target.CompareTag("Enemy")) continue;

            float curDiff = Vector3.Distance(transform.position, target.transform.position);
            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.gameObject;
            }
        }

        return result;
    }
}