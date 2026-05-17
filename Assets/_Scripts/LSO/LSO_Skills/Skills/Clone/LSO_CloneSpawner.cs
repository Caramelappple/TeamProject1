using System.Collections;
using UnityEngine;

public class LSO_CloneSpawner : MonoBehaviour, LSO_ISkill
{
    [SerializeField] private GameObject clonePrefab;
    private bool _canUse = true;
    private float _coolTime = 15f;

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;

        // 플레이어 옆에 소환
        Instantiate(clonePrefab, player.transform.position, Quaternion.identity);

        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}