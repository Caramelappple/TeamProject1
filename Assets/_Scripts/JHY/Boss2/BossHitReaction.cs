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

        // ★ 콘솔 창에서 이 로그가 어떻게 찍히는지 확인하기 위함입니다.
        Debug.Log($"[피격 시도] IsAttacking 상태: {bossAttack?.IsAttacking}");

        if (bossAttack != null && bossAttack.IsAttacking)
        {
            Debug.Log("===> 슈퍼아머 작동으로 피격 애니메이션을 스킵합니다.");
            return;
        }

        Debug.Log("===> 정상 작동: anim.SetTrigger('TakeHit')를 호출합니다.");
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