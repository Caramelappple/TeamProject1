using System.Collections;
using UnityEngine;

public class LSO_Immune : MonoBehaviour, LSO_ISkill
{
    
    private bool _canUse = true;
    
    [SerializeField] private float coolTime = 5f;
    [SerializeField] private float waitTime = 3f;
    
    private GameObject _player;
    

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;

        NKY_SoundManager.Instance.PlaySFX("Immune");
        this._player = player;
        player.GetComponent<MonoBehaviour>().StartCoroutine(CoolTime(3));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false; // 쿨타임 시작
        
        SpriteRenderer sprite = _player.GetComponent<SpriteRenderer>();
        LSO_PlayerMovement movement = _player.GetComponent<LSO_PlayerMovement>();
        LSO_PlayerAttack attack = _player.GetComponent<LSO_PlayerAttack>();
        Health health = _player.GetComponent<Health>();
        Rigidbody2D rigid = _player.GetComponent<Rigidbody2D>();

        KDH_SkillSystem skillSystem = FindFirstObjectByType<KDH_SkillSystem>(); // 김동휘가 건듦

        // 기존 상태 저장
        Color originalColor = sprite.color;
        
        sprite.color = new Color(1, 1, 0.2f, 1);
        if (health != null) health.SetDamageable(false);
        if (movement != null) movement.SetMove(false);
        if (attack != null) attack.SetCanAttack(false);
        if (skillSystem != null) skillSystem.SetCanUseSkill(false); // 김동휘가 건듦 무적상태일 때 스킬이 입력 안 되게
        if (rigid != null) rigid.simulated = false;

        // 지속 시간 대기
        yield return new WaitForSeconds(waitTime);

        // --- [상태 복구] ---
        sprite.color = originalColor;
        if (health != null) health.SetDamageable(true);  // 무적 해제
        if (movement != null) movement.SetMove(true);   // 이동 허용
        if (attack != null) attack.SetCanAttack(true);     // 공격 허용
        if (skillSystem != null) skillSystem.SetCanUseSkill(true); // 김동휘가 건듦 무적상태가 끝나면 스킬 입력 가능
        if (rigid != null) rigid.simulated = true;

        //[남은 쿨타임 대기]
        float remainingCool = coolTime - waitTime;
        if (remainingCool > 0)
        {
            yield return new WaitForSeconds(remainingCool);
        }

        _canUse = true;
    }
}