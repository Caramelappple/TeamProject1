using System;
using System.Collections;
using _Scripts.NKY._EnemyScript.BossPattern;
using UnityEngine;
using Random = UnityEngine.Random;

public class NKY_ChargeSlash : NKY_BossSkill
{
    [Header("스킬 세팅")]
    [SerializeField] private Collider2D hitBox;
    [SerializeField] private GameObject attackSword;
    [SerializeField] private GameObject[] swordPoints;
    [SerializeField] private ParticleSystem[] swordEffect;
    [SerializeField] private GameObject slashEffect;
    
    [Header("스킬 설정")]
    [SerializeField] private int _damage;

    private void Start()
    {
        attackSword.SetActive(false);
        slashEffect.SetActive(false);
        foreach (ParticleSystem particle in swordEffect)
        {
            particle.Stop();
        }
    }

    public override IEnumerator Execute(Transform boss, Transform target)
    {
        yield return StartCoroutine(Move(boss, Vector2.up, 2.5f, 1));
        foreach (GameObject swordPoint in swordPoints)
        {
            yield return PlaySequence(
                SpownSword(attackSword, swordPoint),
                ChargeEffect(swordEffect, attackSword, 1.5f),
                ShowWarn(hitBox, 0.6f, () => hitBox.transform.position),
                WaitUntilOrTime(() => false, 0.5f),
                RotateSword(attackSword, swordPoint.transform.eulerAngles.z + 90f, swordPoint.transform.eulerAngles.z - 90f, -5f),
                Attack(() => _HitBoxController.Cast(hitBox, (target) => HitToDamage(target, _damage))),
                Effect(slashEffect, swordPoint)
            );
            Init(swordPoint);
        }
        yield break;
    }

    private IEnumerator SpownSword(GameObject obj, GameObject point)
    {
        obj.SetActive(true);
        obj.transform.parent = point.transform;
        obj.transform.position = point.transform.position;
        obj.transform.rotation = point.transform.rotation;

        
        yield break;
    }

    private IEnumerator RotateSword(GameObject obj, float fromDuration,  float toDuration, float addDuration)
    {
        float currentDur = obj.transform.rotation.eulerAngles.z;
        float duration = currentDur;
        while (duration <= fromDuration)
        {
            duration += Math.Abs(addDuration * 2);
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
        yield return WaitUntilOrTime(()=>false, 0.6f);
    }

    private IEnumerator ChargeEffect(ParticleSystem[] particles, GameObject obj, float chargeDuration)
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
        
        yield return StartCoroutine(DoShake(obj, chargeDuration, 0.2f));
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
