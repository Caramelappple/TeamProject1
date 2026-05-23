using System.Collections;
using UnityEngine;

public class KHG_PoisonField : MonoBehaviour, LSO_ISkill
{
    [SerializeField] private AudioClip clip;
    
    [SerializeField] private GameObject poisonPrefab;

    [SerializeField] private float spawnDistance = 1.5f;

    private float _coolTime = 3f;
    private bool _canUse = true;

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        
        LSO_SoundManager.Instance.SfxPlay(clip);
        LSO_PlayerMovement movement = player.GetComponent<LSO_PlayerMovement>();
        

        if (!movement) return;

        Vector2 dir = movement.GetFixedLastDir();
        Vector3 spawnPos = player.transform.position + (Vector3)(dir * spawnDistance);
        
        
        GameObject poison = Instantiate(poisonPrefab, spawnPos, Quaternion.identity);
        poison.GetComponent<KHG_PoisonDamage>().Init(player.GetComponent<Health>());

        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }
    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}