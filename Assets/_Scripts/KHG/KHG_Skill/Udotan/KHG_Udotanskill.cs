using UnityEngine;
using System.Collections;

public class KHG_Udotanskill : MonoBehaviour,LSO_ISkill
{
    [SerializeField] private AudioClip clip;
    
    [SerializeField] private GameObject bulletPrefab; 
    private GameObject _bullet;
    private Rigidbody2D _rigid;
    private LSO_PlayerMovement _movement;
    private float _speed = 3f;
    
    [SerializeField] public float skillCooldown = 3f; 
    private bool isSkillReady = true; 
    
    public void UseSkill(GameObject player)
    {
        if (!isSkillReady) return;
        isSkillReady = false;
        
        LSO_SoundManager.Instance.SfxPlay(clip);
        _bullet = Instantiate(bulletPrefab, player.transform.position, Quaternion.identity);
        _rigid = _bullet.GetComponent<Rigidbody2D>();
        _movement = player.GetComponent<LSO_PlayerMovement>();
        _bullet.GetComponent<KHG_CircleBullet>().Init(player.GetComponent<Health>(), _speed);
        
        
        _rigid.linearVelocity = _movement.GetLastDir().normalized * _speed;
        //Debug.Log(_rigid.linearVelocity);
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(skillCooldown));
    }

    public IEnumerator CoolTime(float time)
    {
            
            
            yield return new WaitForSeconds(time);
            isSkillReady = true;
    }
}