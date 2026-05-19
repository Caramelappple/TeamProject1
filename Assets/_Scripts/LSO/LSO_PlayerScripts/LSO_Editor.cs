using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class LSO_Editor : MonoBehaviour
{
    public static LSO_Editor Instance;
    
    public PostProcessVolume postProcessVolume;
    public ColorGrading colorGrading;
    public Bloom bloom;
    public Vignette vignette;
    public DepthOfField depthOfField;

    private void Awake()
    {
        Instance = this;
        postProcessVolume = GetComponent<PostProcessVolume>();
        colorGrading = postProcessVolume.profile.GetSetting<ColorGrading>();
        bloom = postProcessVolume.profile.GetSetting<Bloom>();
        vignette = postProcessVolume.profile.GetSetting<Vignette>();
        depthOfField = postProcessVolume.profile.GetSetting<DepthOfField>();
    }

    private void Start()
    {
        vignette.active = false;
        colorGrading.active = true;
    }

    public void Register(Health health)
    {
        health.OnDamage += (data) => SetHit(health);
    }

    private void SetHit(Health health)
    {
        StartCoroutine(SetHitCoroutine(health));
    }

    private IEnumerator SetHitCoroutine(Health health)
    {
        if (!health.gameObject.CompareTag("Player")) yield break;
        
        CameraShake.instance.Shake(0.15f, 0.12f);
        vignette.intensity.value = 0.55f;
        vignette.active = true;
        yield return new WaitForSeconds(0.2f);
        while (vignette.intensity.value > 0f)
        {
            yield return null;
            vignette.intensity.value -= 0.007f;
        }
        vignette.active = false;
    }

    public IEnumerator SetBlur()
    {
        depthOfField.focalLength.value = 0f;
        depthOfField.active = true;
        Tween tween = DOTween.To(() => depthOfField.focalLength.value, x => depthOfField.focalLength.value = x, 250f, 0.3f).SetEase(Ease.OutBack);
        yield return tween.WaitForCompletion();
        DOTween.To(() => depthOfField.focalLength.value, x => depthOfField.focalLength.value = x, 0f, 0.6f).SetEase(Ease.OutExpo);
        vignette.active = false;
    }
}
