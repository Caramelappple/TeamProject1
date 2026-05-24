using System.Collections;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;

public class NKY_ChargeSlash : NKY_BossSkill
{
    private static readonly int Stun = Animator.StringToHash("isStun");

    [Header("스킬 세팅")]
    [SerializeField] private Collider2D hitBox;
    [SerializeField] private GameObject attackSword;
    [SerializeField] private GameObject[] swordPoints;
    [SerializeField] private ParticleSystem[] swordParticle;
    [SerializeField] private GameObject slashEffect;
    
    [Header("스킬 설정")]
    [field: SerializeField] public override float DamageScale { get; protected set; } = 3f;
    
    //private int _damage;

    private void Start()
    {
        attackSword.SetActive(false);
        slashEffect.SetActive(false);
        foreach (ParticleSystem particle in swordParticle)
        {
            particle.Stop();
        }
        _damage = (int)DamageScale * _bossBrain.Damage;
    }

    public override IEnumerator Execute(Transform boss, Transform target)
    {
        yield return StartCoroutine(Move(boss, Vector2.up, 2.5f, 1));
        foreach (GameObject swordPoint in swordPoints)
        {
            yield return PlaySequence(
                SpownSword(attackSword, swordPoint),
                ChargeEffect(swordParticle, attackSword, 0.8f),
                ShowWarn(hitBox, 0.6f, () => hitBox.transform.position),
                WaitUntilOrTime(() => false, 0.3f),
                RotateSword(attackSword, swordPoint.transform.eulerAngles.z + 90f, swordPoint.transform.eulerAngles.z - 90f, -10f),
                
                Attack(() => _HitBoxController.Cast(hitBox, (hitTarget) => HitToDamage(boss.gameObject, hitTarget.gameObject, _damage))),
                Effect(slashEffect, swordPoint)
            );
            Init(swordPoint);
        }
        Anim.SetBool(Stun, true);
        yield return WaitUntilOrTime(()=>false, 4f);
        Anim.SetBool(Stun, false);
        yield return StartCoroutine(WaitAnim("StandUp", 1f));
    }

    public override void EndSkill()
    {
        foreach (GameObject swordPoint in swordPoints)
        {
            Init(swordPoint);
        }
        foreach (ParticleSystem particle in swordParticle)
        {
            particle.Stop();
            particle.gameObject.SetActive(false);
        }
    }

    private IEnumerator SpownSword(GameObject obj, GameObject point)
    {
        obj.SetActive(true);
        obj.transform.parent = point.transform;
        obj.transform.position = point.transform.position;
        obj.transform.rotation = point.transform.rotation;
        NKY_SoundManager.Instance.PlaySFX("SwordSpawn");
        yield break;
    }

    private IEnumerator RotateSword(GameObject obj, float fromDuration,  float toDuration, float addDuration)
    {
        float currentDur = obj.transform.rotation.eulerAngles.z;
        float duration = currentDur;
        while (duration <= fromDuration)
        {
            duration += addDuration * -1;
            obj.transform.rotation = Quaternion.Euler(0f, 0f, duration);
            yield return  null;
        }
        duration = fromDuration;
        obj.transform.rotation = Quaternion.Euler(0f, 0f, duration);
        yield return WaitUntilOrTime(()=>false, 0.5f);
        while (duration > toDuration)
        {
            duration += addDuration;
            obj.transform.Rotate(0, 0, addDuration);
            yield return  null;
        }
        yield break;
    }

    private IEnumerator Effect(GameObject obj, GameObject point)
    {
        obj.transform.position =  point.transform.position;
        obj.transform.rotation =  Quaternion.Euler(0f, 0f, point.transform.rotation.eulerAngles.z);
        obj.SetActive(true);
        NKY_SoundManager.Instance.PlaySFX("Slash");
        yield return WaitUntilOrTime(()=>false, 0.6f);
    }

    private IEnumerator ChargeEffect(ParticleSystem[] particles, GameObject obj, float chargeDuration)
    {
        NKY_SoundManager.Instance.PlaySFX("Charge");
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
        
        yield return StartCoroutine(DoShake(obj, chargeDuration, 0.3f));
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
        }
        yield break;
    }

    private void Init(GameObject obj)
    {
        attackSword.SetActive(false);
        slashEffect.SetActive(false);
        obj.transform.rotation = Quaternion.Euler(0f, 0f, obj.transform.rotation.eulerAngles.z);
    }
}
