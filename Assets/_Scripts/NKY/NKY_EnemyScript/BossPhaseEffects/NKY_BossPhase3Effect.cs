using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.NKY._EnemyScript.BossPhaseEffects
{
    public class NKY_BossPhase3Effect : NKY_PhaseEffect
    {
        [SerializeField] private GameObject impectRing;
        [SerializeField] private float ringSize;
        [SerializeField] private float ringDuration;
        
        private SpriteRenderer _bossRingSr;

        private void Start()
        {
            _bossRingSr =  impectRing.GetComponent<SpriteRenderer>();
            impectRing.SetActive(false);
        }

        public override IEnumerator PlayPhaseEffect()
        {
            _bossAnimator.Play("Dead");
            NKY_SoundManager.Instance.PlaySFX("StunClothes");
            NKY_SoundManager.Instance.PlaySFX("ClashGlass");
            yield return WaitAnim(_bossAnimator, "Dead", 0.6f);
            ImpactRingEffect();
            _bossAnimator.Play("Resurrection");
            NKY_SoundManager.Instance.PlaySFX("AttachGlass");
            yield return WaitUntilOrTime(() => false, 3f);
            _bossAnimator.Play("StandUp");
            yield return WaitAnim(_bossAnimator, "StandUp", 1f);
            impectRing.SetActive(false);
        }

        public override IEnumerator EndPhaseEffect()
        {
            yield break;
        }

        private void ImpactRingEffect()
        {
            impectRing.SetActive(true);
            NKY_SoundManager.Instance.PlaySFX("Impact");
            impectRing.transform.DOScale(new Vector2(ringSize, ringSize), ringDuration);
            _bossRingSr.DOFade(0, ringDuration);
            Camera.main.transform.DOShakePosition(ringDuration, 1.3f);
        }
    }
}