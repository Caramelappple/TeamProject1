using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.NKY._EnemyScript.BossPhaseEffects
{
    public class NKY_BossPhase2Effect : NKY_PhaseEffect
    {
        private static readonly int IsStun = Animator.StringToHash("isStun");

        [Header("이펙트 세팅")]
        [SerializeField] private GameObject bloodEffect;
        [SerializeField] private GameObject fireEffect;

        private Animator _effectAnim = null;
        
        
        private void Start()
        {
            bloodEffect.SetActive(false);
            fireEffect.SetActive(false);
        }

        public override IEnumerator PlayPhaseEffect()
        {
            StartCoroutine(PlayEffect(bloodEffect, "BloodEffect"));
            _bossBrain.Anim.SetBool(IsStun, true);
            yield return StartCoroutine(WaitUntilOrTime(() => false, 0.8f));
            StartEffect(fireEffect, "FireEffect", true);
            yield return StartCoroutine(WaitUntilOrTime(() => false, 2f));
            _bossBrain.Anim.SetBool(IsStun, false);
        }

        public override IEnumerator EndPhaseEffect()
        {
            yield return EndEffect(fireEffect, "FireEffect");
        }

        private IEnumerator PlayEffect(GameObject effect, string animName)
        {
            Animator effectAnim = effect.GetComponent<Animator>();
            StartEffect(effect, animName, false,  effectAnim);
            yield return EndEffect(effect, animName, effectAnim);
        }

        private void StartEffect(GameObject effect, string animName, bool doScale = false, Animator effectAnim = null)
        {
            if(effectAnim == null)
                effectAnim = effect.GetComponent<Animator>();
            
            if (doScale)
            {
                effect.transform.localScale *= 0.1f; 
                effect.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
            }
            effect.SetActive(true);
            effectAnim.Play(animName);
        }

        private IEnumerator EndEffect(GameObject effect ,string animName, Animator effectAnim = null)
        {
            if (effectAnim == null) yield break;
            
            yield return WaitAnim(effectAnim, animName, 1);
            effect.SetActive(false);
            _effectAnim = null;
        }
    }
}