using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSO_Test2 : MonoBehaviour,LSO_ISkill
{
    private float coolTime = 3f;
    private bool canUseSkill = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UseSkill(GameObject player)
    {
        if (!canUseSkill) return;
        Debug.Log("Skill2 used");
        //StartCoroutine(CoolTime(coolTime));
    }
    
    public IEnumerator CoolTime(float time)
    {
        canUseSkill = false;
        yield return new WaitForSeconds(time);
        canUseSkill = true;
    }
}
