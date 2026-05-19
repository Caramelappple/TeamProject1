using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LSO_Slow : MonoBehaviour, LSO_ISkill
{
    [SerializeField] private GameObject effect;
    private GameObject _effectInstance;
    private Animator _animator;

    private bool _canUse = true;
    private float _coolTime = 30f;
    private float _waitTime = 10f;

    private Tween _scaleTween;
    private Tween _satTween;

    public void UseSkill(GameObject player)
    {
        if (!_canUse) return;
        _effectInstance = Instantiate(effect, player.transform.position, Quaternion.identity);
        _animator = _effectInstance.GetComponent<Animator>();

        StartCoroutine(LSO_Editor.Instance.SetBlur());
        SetScale(0.5f);
        SetSat(-100f);

        StartCoroutine(CoolTime(_coolTime));
    }

    public IEnumerator CoolTime(float time)
    {
        _canUse = false;
        yield return new WaitForSecondsRealtime(_waitTime);

        SetScale(1f);
        SetSat(0f);
        StartCoroutine(LSO_Editor.Instance.SetBlur());

        yield return new WaitForSecondsRealtime(time); // timeScale 영향 안 받도록
        _canUse = true;
    }

    private void FixedUpdate()
    {
        if (!_animator || !_effectInstance) return;

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Animation-magic-12-a") && stateInfo.normalizedTime >= 0.95f)
        {
            Destroy(_effectInstance);
        }
    }

    private void SetSat(float value)
    {
        _satTween?.Kill();
        _satTween = DOTween.To(
            () => LSO_Editor.Instance.colorGrading.saturation.value,
            x => LSO_Editor.Instance.colorGrading.saturation.value = x,
            value, 1f
        ).SetUpdate(true);
    }

    private void SetScale(float value)
    {
        _scaleTween?.Kill();
        _scaleTween = DOTween.To(
            () => Time.timeScale,
            x => Time.timeScale = x,
            value, 1f
        ).SetUpdate(true);
    }

    private void OnDestroy()
    {
        // 게임오브젝트 삭제 시 모든 Tween 강제 종료
        _scaleTween?.Kill();
        _satTween?.Kill();
        

        // 혹시 진행 중이던 값 원상복구
        Time.timeScale = 1f;
        if (LSO_Editor.Instance != null)
            LSO_Editor.Instance.colorGrading.saturation.value = 0f;
    }
}