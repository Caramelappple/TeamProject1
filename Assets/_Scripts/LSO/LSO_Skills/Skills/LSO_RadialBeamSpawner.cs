using System.Collections;
using UnityEngine;

public class LSO_RadialBeamSpawner : MonoBehaviour,LSO_ISkill
{ 
    private GameObject _player;
    private bool _canUse = true;
    
    private float _coolTime = 5f;
    private float _minWaitTime = 0.1f;
    private float _maxWaitTime = 0.8f;
    private int _randomDegree;
    private int _count = 20;//내리칠 횟수
    private float _range = 2;
    
    private Vector3 _spawnPos;
        
    [SerializeField]private GameObject effect;
    private GameObject _effectInstance;//이펙트 복제한것
    private Animator _animator;
    private SpriteRenderer _sprite;
    
    public void UseSkill(GameObject player)
    { if (!_canUse) return;
        _player = player;
        
        _randomDegree = Random.Range(-180, 180);
        Vector3 _spawnRot = new Vector3(0,0,_randomDegree);
        _spawnPos = new Vector3(player.transform.position.x, player.transform.position.y+ _range, player.transform.position.z);
        
        _effectInstance = Instantiate(effect, _spawnPos, Quaternion.identity);
        _effectInstance.transform.Rotate(_spawnRot);
        _effectInstance.transform.parent = transform;
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }
    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        for (int i = 0; i < _count; i++)
        {
            _randomDegree = Random.Range(-180, 180);
            Vector3 _spawnRot = new Vector3(0, 0, _randomDegree);
            _effectInstance.transform.Rotate(_spawnRot);
            
            //yield return new WaitForSeconds();
        }
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
}