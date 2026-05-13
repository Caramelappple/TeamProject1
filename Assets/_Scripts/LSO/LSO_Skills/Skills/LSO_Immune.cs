using System.Collections;
using UnityEngine;

public class LSO_Immune : MonoBehaviour, LSO_ISkill
{
    private GameObject _player;
    private SpriteRenderer _sprite;
    private LSO_PlayerMovement _playerMovement;
    private LSO_PlayerAttack _playerAttack;

    private bool _canUse = true;
    private bool _canMove = true;
    
    private float _coolTime = 5;
    private float _waitTime = 3;
    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        
        _player = player;
        _sprite = player.GetComponent<SpriteRenderer>();
        _playerMovement = player.GetComponent<LSO_PlayerMovement>();
        _playerAttack = player.GetComponent<LSO_PlayerAttack>();
        
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        
        
        Debug.Log("dsfa");
        Color originalColor = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, _sprite.color.a);//기존 색 저장
        Health health = _player.GetComponent<Health>();//체력 가져오기
        
        _sprite.color = new Color(1, 1, 0.2f, 1);//색 바꾸기
        health.SetDamageable(false);//무적으로 바꾸기
        _playerMovement.SetMove(false);//못 움직이게 하기 및 스킬 착용 및 사용 막기
        _playerAttack.SetAttack(false);//공격 못하게 하기
        
        yield return new WaitForSeconds(_waitTime);//유지시간
        
        _sprite.color = originalColor;
        health.SetDamageable(false);
        _playerAttack.SetAttack(false);//공격 못하게 하기
        _playerMovement.SetMove(true);
        
        yield return new WaitForSeconds(time);
        _canUse = true;
    }
}
