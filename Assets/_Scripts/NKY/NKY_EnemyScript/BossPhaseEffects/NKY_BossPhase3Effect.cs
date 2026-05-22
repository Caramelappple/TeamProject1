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
            yield return WaitAnim(_bossAnimator, "Dead", 0.6f);
            //임펙트링의 사이즈를 DoScale을 이용해 사이즈를 늘리고 투명화를 하여 천천히 사라지도록 하는 코루틴을 만들어서 예일드 리턴으로 반환
            ImpactRingEffect();
            _bossAnimator.Play("Resurrection");
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
            impectRing.transform.DOScale(new Vector2(ringSize, ringSize), ringDuration);
            _bossRingSr.DOFade(0, ringDuration);
            Camera.main.transform.DOShakePosition(ringDuration, 1.3f);
        }
    }
}