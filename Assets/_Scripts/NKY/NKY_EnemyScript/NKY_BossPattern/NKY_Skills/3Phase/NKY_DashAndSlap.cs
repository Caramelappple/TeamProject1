using System.Collections;
using _Scripts.NKY._EnemyScript.BossPattern;
using DG.Tweening;
using UnityEngine;

public class NKY_DashAndSlap : NKY_BossSkill
{
    private static readonly int IsMove = Animator.StringToHash("isMove");

    [Header("스킬 세팅")]
    [SerializeField] private Collider2D col;
    [SerializeField] private GameObject dustEffect;
    
    [Header("스킬 설정")]
    [SerializeField] private float dashSpeed = 7f;
    [field: SerializeField] public override float DamageScale { get; protected set; } = 1f;
    
    //private int _damage;

    private void Start()
    {
        _damage = (int)(DamageScale * _bossBrain.Damage);
        dustEffect.SetActive(false);
    }

    public override IEnumerator Execute(Transform boss, Transform target)
    {
        Vector3 dir = target.position - boss.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += 90f;
        yield return PlaySequence(MoveAnim(true, dir, boss),
            ShowWarn(0, new Vector2(2f, (target.position - boss.transform.position).magnitude + 2f), 2f, 
            () => Vector3.Lerp(boss.position, target.position, 0.5f), angle),
            WaitUntilOrTime(() => false, 0.7f),
            PlaySound("Rush"),
            ConstantMoveTo(boss, target.position, dashSpeed),
            ShowWarn(col, 0.4f, () => _shadow.transform.position),
            MoveAnim(false, dir, boss),
            WaitUntilOrTime(() => false, 0.3f),
            Move(boss, Vector2.up, 0.5f, 0.2f),
            Move(boss, Vector2.down, 0.5f, 0.1f),
            Attack(() => _HitBoxController.Cast(col,_shadow.transform.position, (hitTarget) => HitToDamage(boss.gameObject, hitTarget.gameObject, _damage))),
            PlayDustEffect(dustEffect, boss)
            );
    }

    private IEnumerator PlaySound(string soundName)
    {
        NKY_SoundManager.Instance.PlaySFX(soundName);
        yield break;
    }

    public override void EndSkill()
    {
        dustEffect.SetActive(false);
        _bossBrain.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        Anim.SetBool(IsMove, false);
    }

    private IEnumerator PlayDustEffect(GameObject effect, Transform pos)
    {
        NKY_SoundManager.Instance.PlaySFX("Earthquake");
        Animator effectAnim =  effect.GetComponent<Animator>();
        effect.transform.position = pos.position;
        effect.SetActive(true);
        Camera.main.transform.DOShakePosition(0.5f, 1.5f);
        effectAnim.Play("PlayDustEffect");
        yield return WaitAnim(effectAnim, "PlayDustEffect", 0.9f);
        effect.SetActive(false);
    }

    private IEnumerator MoveAnim(bool isMove, Vector2 dir, Transform target)
    {
        Anim.SetBool(IsMove, isMove);
        if (!isMove)
        {
            target.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (dir.x < 0)
        {
            target.rotation = Quaternion.Euler(0, 180, 0);
        }
            

        yield break;
    }
}
