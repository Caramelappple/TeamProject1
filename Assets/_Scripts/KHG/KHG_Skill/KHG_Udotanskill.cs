using UnityEngine;
using System.Collections;

public class KHG_Udotanskill : MonoBehaviour,LSO_ISkill
{
    [SerializeField] private GameObject bulletPrefab; 
    private GameObject _bullet;
    private Rigidbody2D _rigid;
    private LSO_PlayerMovement _movement;
    
    public float skillCooldown = 3f; 
    private bool isSkillReady = true; 
    
    public void UseSkill(GameObject player)
    {
        if (!isSkillReady) return;
        isSkillReady = false;
        
        _bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        _rigid = GetComponent<Rigidbody2D>();
        _movement = player.GetComponent<LSO_PlayerMovement>();
        
        _rigid.linearVelocity = _movement.GetLastDir();
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(skillCooldown));
    }

    public IEnumerator CoolTime(float time)
    {
            
            
            yield return new WaitForSeconds(time);
            isSkillReady = true;
    }
}