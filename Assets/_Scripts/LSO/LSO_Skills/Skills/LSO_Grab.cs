using System.Collections;
using TMPro;
using UnityEngine;

public class LSO_Grab : MonoBehaviour,LSO_ISkill
{
    private bool _canUse = true;
    private GameObject _player;
    private GameObject _target;
    private Rigidbody2D _targetRigid;
    
    //[SerializeField] private GameObject effect;
    //private GameObject _effectInstance;
    
    [SerializeField]private float coolTime = 5f;
    [SerializeField]private int damage = 80;
    
    private Collider2D[] _targets;
    private GameObject _nearestTarget;
    private readonly float _range = 200;
    private readonly float _diff = 100;
    private readonly float _skillDiff = 0.8f;
    private readonly float _speed = 5;
    
    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        Debug.Log("UseGrab");
        _player = player;
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        
        _target = GetNearest();
        Debug.Log(_target.name);
        
        if (_target.TryGetComponent<Health>(out Health targetHealth))
        {
            DamageData data = new DamageData(targetHealth, damage);
            targetHealth.GetDamage(data);
            
            _targetRigid = _target.GetComponent<Rigidbody2D>();
            //_targetRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
        while (Vector3.Distance(_player.transform.position, _target.transform.position) > _skillDiff)
        {
            _targetRigid.linearVelocity = (_player.transform.position - _target.transform.position).normalized * _speed;
            yield return null;
        }
        _targetRigid.linearVelocity = Vector2.zero;
        
        yield return new WaitForSeconds(time);
        
        _canUse = true;
    }
    
    private GameObject GetNearest()
    {
        _targets = Physics2D.OverlapCircleAll(_player.transform.position, _range);
        
        GameObject result = null;
        float diff = this._diff;
        
        foreach (Collider2D target in _targets)
        {
           if (!target.CompareTag("Enemy")) continue;
            
            Vector3 myPos = transform.position;
            Vector3 targetPos =  target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.gameObject;
            }
        }
        return result;
    }
}
