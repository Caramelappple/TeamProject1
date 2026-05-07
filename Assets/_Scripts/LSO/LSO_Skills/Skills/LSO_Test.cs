using System.Collections;
using UnityEngine;

public class LSO_Test : MonoBehaviour,LSO_ISkill
{
    private float _coolTime = 5f;
    private bool _canUseSkill = true;
   
    public void UseSkill(GameObject player)
    {
        if (!_canUseSkill) return;
        Debug.Log(player.gameObject.name);
        StartCoroutine(CoolTime(_coolTime));
    }
    
    public IEnumerator CoolTime(float time)
    {
        _canUseSkill = true;
        yield return new WaitForSeconds(time);
        _canUseSkill = false;
    }
}
