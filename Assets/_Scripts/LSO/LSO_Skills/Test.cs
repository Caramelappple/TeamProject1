using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour,ISkill
{
    private float coolTime = 5f;
    private bool canUseSkill = true;
   
    public void UseSkill()
    {
        if (!canUseSkill) return;
        Debug.Log("Skill1 used");
        //StartCoroutine(CoolTime(coolTime));
    }
    
    public IEnumerator CoolTime(float time)
    {
        canUseSkill = true;
        yield return new WaitForSeconds(time);
        canUseSkill = false;
    }
}
