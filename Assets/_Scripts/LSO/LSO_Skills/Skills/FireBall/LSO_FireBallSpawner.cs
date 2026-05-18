using System.Collections;
using UnityEngine;

public class LSO_FireBallSpawner : MonoBehaviour,LSO_ISkill
{
    private LSO_PlayerMovement _movement;
    private Vector3 _lastDir;
     
    [SerializeField] private GameObject fireballPrefab;
    private GameObject _fireball;
    
    private bool _canUse = true;
    private float _coolTime = 5f;
    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        
        _movement = player.GetComponent<LSO_PlayerMovement>();
        _lastDir = _movement.GetFixedLastDir();
        float angle = Mathf.Atan2(_lastDir.y, _lastDir.x) * Mathf.Rad2Deg;
        
        _fireball = Instantiate(fireballPrefab, player.transform.position, Quaternion.AngleAxis(angle+90, Vector3.forward));
        _fireball.GetComponent<LSO_FireBall>().Init(_lastDir);
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}
