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

        private Animator _fireAnim = null;
        
        
        private void Start()
        {
            fireEffect.SetActive(false);
            bloodEffect.SetActive(false);
        }

        public override IEnumerator PlayPhaseEffect()
        {
            Camera.main.transform.DOShakePosition(0.6f, 1.6f);
            NKY_SoundManager.Instance.PlaySFX("BloodEffect");
            StartCoroutine(PlayEffect(bloodEffect, "BloodEffect", 0.6f));
            NKY_SoundManager.Instance.PlaySFX("StunClothes");
            _bossBrain.Anim.SetBool(IsStun, true);
            yield return StartCoroutine(WaitUntilOrTime(() => false, 0.8f));
            NKY_SoundManager.Instance.PlaySFX("FireOn");
            StartEffect(fireEffect, "FireEffect", true, _fireAnim);
            yield return StartCoroutine(WaitUntilOrTime(() => false, 2f));
            _bossBrain.Anim.SetBool(IsStun, false);
        }

        public override IEnumerator EndPhaseEffect()
        {
            
            yield return EndEffect(fireEffect, 0.7f);
        }

        private IEnumerator PlayEffect(GameObject effect, string animName, float duration)
        {
            Animator effectAnim = effect.GetComponent<Animator>();
            StartEffect(effect, animName, false,  effectAnim);
            yield return EndEffect(effect, duration);
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

        private IEnumerator EndEffect(GameObject effect ,float duration)
        {
            effect.GetComponent<SpriteRenderer>().DOFade(0, duration);
            yield return WaitUntilOrTime(() => false, duration);
            effect.SetActive(false);
        }
    }
}