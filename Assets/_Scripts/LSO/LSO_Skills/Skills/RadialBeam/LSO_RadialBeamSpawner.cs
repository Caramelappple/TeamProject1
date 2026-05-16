using System.Collections;
using UnityEngine;

public class LSO_RadialBeamSpawner : MonoBehaviour,LSO_ISkill
{ 
    private GameObject _player;
    private bool _canUse = true;
    
    private float _coolTime = 5f;
    private float _waitTime = 0.2f;
    
    private int _randomDegree;
    private int _count = 15;
    [SerializeField]private GameObject projectilePrefab;
    private int _projectileCount = 6;
    private float _spreadAngle = 360f;
    private float _range = 2;
    
    private float _minWaitTime = 0.2f;
    private float _maxWaitTime = 0.5f;
    
    private Vector3 _spawnPos;
        
    [SerializeField]private GameObject effect;
    private GameObject _effectInstance;//이펙트 복제한것
    private Animator _animator;
    private bool _onEnd = false;
    
    public void UseSkill(GameObject player)
    { if (!_canUse) return;
        _player = player;
        
        _randomDegree = Random.Range(-180, 180);
        Vector3 spawnRot = new Vector3(0,0,_randomDegree);
        _spawnPos = new Vector3(player.transform.position.x, player.transform.position.y+ _range, player.transform.position.z);
        
        _effectInstance = Instantiate(effect, _spawnPos, Quaternion.identity);
        _effectInstance.transform.Rotate(spawnRot);
        _effectInstance.transform.parent = transform;
        _animator = _effectInstance.GetComponent<Animator>();
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }
    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        for (int i = 0; i < _count; i++)
        {
            //이펙트 돌리기
            _randomDegree = Random.Range(-180, 180);
            Vector3 spawnRot = new Vector3(0, 0, _randomDegree);
            _effectInstance.transform.Rotate(spawnRot);
            
            StartCoroutine(FireProjectiles());
            
            yield return new WaitForSeconds(_waitTime);
        }
        _animator.SetTrigger("End");
        _onEnd = true;
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
    
    private void FixedUpdate()
    {
        if (!_animator || !_effectInstance) return;
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Beam2") && stateInfo.normalizedTime >= 0.95f)
        {
            Destroy(_effectInstance);//끝나는 애니메이션이 끝나면 오브젝트 삭제
        }
    }
    
    private IEnumerator FireProjectiles()
    {
        Transform firePoint = _effectInstance.transform;

        Vector2 dirToPlayer = (_spawnPos - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
        
        if (_projectileCount <= 1)
        {
            if (!firePoint) yield break;
                
            Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0f, 0f, baseAngle));
            yield break;
        }
        
        for (int i = 0; i < _projectileCount; i++)
        {
            if (!firePoint || _onEnd) yield break;
            
            float currentAngle = Random.Range(0f, 360f);
            Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0f, 0f, currentAngle));
            yield return new WaitForSeconds(Random.Range(_minWaitTime, _maxWaitTime));
        }
    }
}