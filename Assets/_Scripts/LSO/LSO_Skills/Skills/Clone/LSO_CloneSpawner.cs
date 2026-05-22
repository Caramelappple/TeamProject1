using System.Collections;
using UnityEngine;

public class LSO_CloneSpawner : MonoBehaviour, LSO_ISkill
{
    [SerializeField] private AudioClip clip;
    
    [SerializeField] private GameObject clonePrefab;
    private GameObject _clone;

    private LSO_PlayerMovement _movement;
    
    private bool _canUse = true;
    private float _coolTime = 15f;

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;

        // 플레이어 옆에 소환
        _movement = player.GetComponent<LSO_PlayerMovement>();
        LSO_SoundManager.Instance.SfxPlay(clip);
        _clone = Instantiate(clonePrefab, player.transform.position, Quaternion.identity);
        _clone.GetComponent<LSO_Clone>().Init(_movement);
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}