using UnityEngine;

public class BossHitReaction : MonoBehaviour
{
    private Health health;
    private Animator anim;
    private bool isDead;

    // ★ 추가됨: 보스의 공격 상태를 가져오기 위한 변수
    private BossAttack2 bossAttack;

    [SerializeField] private MonoBehaviour[] disableScripts;
    [SerializeField] private Collider2D[] disableColliders;
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();

        // ★ 추가됨: 시작할 때 같은 오브젝트에 있는 BossAttack2를 찾아서 연결해 둡니다.
        bossAttack = GetComponent<BossAttack2>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (health != null)
            health.OnDamage += HandleDamage;
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnDamage -= HandleDamage;
    }

    private void HandleDamage(DamageResultData data)
    {
        if (anim == null || isDead) return;

        if (health.IsDestroyed)
        {
            Die();
            return;
        }

        // ★ 추가됨: 만약 보스가 지금 공격 중(IsAttacking == true)이라면,
        // 데미지는 이미 Health에서 받았으니, 아래의 "TakeHit" 애니메이션을 틀지 않고 여기서 끝냅니다!
        if (bossAttack != null && bossAttack.IsAttacking)
        {
            Debug.Log("보스가 공격 중이라 끄떡없습니다! (슈퍼아머)");
            return;
        }

        // 평소에 맞을 때만 "TakeHit" 재생
        anim.SetTrigger("TakeHit");
    }

    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Dead");

        if (health != null)
            health.SetDamageable(false);

        foreach (MonoBehaviour script in disableScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        foreach (Collider2D col in disableColliders)
        {
            if (col != null)
                col.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }
    }
}