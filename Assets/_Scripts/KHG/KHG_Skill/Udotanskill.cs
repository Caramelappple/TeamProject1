using UnityEngine;
using System.Collections;

public class KHG_Udotanskill : MonoBehaviour,LSO_ISkill
{
    public GameObject bulletPrefab; 
    public float skillCooldown = 3f; 
    private bool isSkillReady = true; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isSkillReady)
        {
            StartCoroutine(CircleBulletSkill());
        }
    }

    IEnumerator CircleBulletSkill()
    {
        isSkillReady = false; 

       
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        

        yield return new WaitForSeconds(skillCooldown);

        isSkillReady = true; 
    }

    public void UseSkill(GameObject player)
    {
        
    }

    public IEnumerator CoolTime(float time)
    {
            isSkillReady = false;
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(skillCooldown);

            isSkillReady = true;
    }
}