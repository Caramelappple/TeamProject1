using System.Collections;
using UnityEngine;

public class PunchSpawner : MonoBehaviour,LSO_ISkill
{
    [SerializeField] private GameObject Effect;
    
    private LSO_PlayerMovement _movement;
    private bool _canUse = true;
    private float _coolTime = 8f;
    private float _distance = 1.5f;
    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        _canUse = false;
        
        _movement = player.GetComponent<LSO_PlayerMovement>();
        
        Instantiate(Effect, player.transform.position+_movement.GetLastDir() * _distance, Quaternion.identity);
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}
