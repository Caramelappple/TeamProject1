using System.Collections;
using _Scripts.NKY._EnemyScript.BossPattern;
using _Scripts.NKY.Manager;
using UnityEngine;

public class NKY_SlashRush : NKY_BossSkill
{
    [SerializeField] private GameObject slash;
    [SerializeField] private Collider2D slashCollider;
    
    [Header("스킬 설정")]
    [SerializeField] private int attackCount = 15;
    [SerializeField] private float moveDistance = 3f;
    [field: SerializeField] public override float DamageScale { get; protected set; }
    
    //private int _damage;

    private void Start()
    {
        _damage = (int)(DamageScale * _bossBrain.Damage);
        slash.SetActive(false);
    }

    public override IEnumerator Execute(Transform boss, Transform target)
    {
        Transform targetTransform = target;
        Transform bossTransform = boss;
        for (int i = 0; i < attackCount; i++)
        {
            Vector2 dir = (targetTransform.position - bossTransform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle -= 90f;
            yield return PlaySequence(
                Move(bossTransform, dir, moveDistance * 0.1f, 0.2f),
                SlashEffect(slash, bossTransform.position, angle, slashCollider),
                Attack(() => _HitBoxController.Cast(slashCollider,(hitTarget) => HitToDamage(boss.gameObject, hitTarget.gameObject, _damage)))
                );
        }
    }

    public override void EndSkill()
    {
        slash.SetActive(false);
    }

    private IEnumerator SlashEffect(GameObject effect, Vector2 pos, float angle, Collider2D col)
    {
        Animator effectAnim = effect.GetComponentInChildren<Animator>();
        effect.transform.position = pos;
        col.transform.rotation = Quaternion.Euler(0f, 0f, angle + 180f);
        effect.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        effect.SetActive(true);
        effectAnim.Play("SlashEffect");
        NKY_SoundManager.Instance.PlaySFX("Swing");
        yield return WaitAnim(effectAnim, "SlashEffect", 0.9f);
        effect.SetActive(false);
    }
}
