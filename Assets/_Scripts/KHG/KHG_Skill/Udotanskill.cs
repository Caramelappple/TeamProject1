using UnityEngine;
using System.Collections;

public class KHG_Udotanskill : MonoBehaviour,LSO_ISkill
{
    public GameObject bulletPrefab; 
    public float skillCooldown = 3f; 
    private bool isSkillReady = true; 
    public void UseSkill(GameObject player)
    {
        if (!isSkillReady) return;
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(skillCooldown));
        
    }

    public IEnumerator CoolTime(float time)
    {
            isSkillReady = false;
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(time);
            isSkillReady = true;
    }
}