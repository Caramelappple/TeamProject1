using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

public class LSO_Editor : MonoBehaviour
{
    public static LSO_Editor Instance;
    
    public PostProcessVolume postProcessVolume;
    public ColorGrading colorGrading;
    public Bloom bloom;
    public Vignette vignette;

    private void Awake()
    {
        Instance = this;
        postProcessVolume = GetComponent<PostProcessVolume>();
        colorGrading = postProcessVolume.profile.GetSetting<ColorGrading>();
        bloom = postProcessVolume.profile.GetSetting<Bloom>();
        vignette = postProcessVolume.profile.GetSetting<Vignette>();
    }

    private void Start()
    {
        vignette.active = false;
        colorGrading.active = true;
    }

    public void Register(Health health)
    {
        health.OnDamage += (data) => SetHit(health);
        health.OnDamage += (data) => Debug.Log($"<color=red>{this.gameObject}가 {data.giver}로부터 {data.damage}만큼 대미지를 받았습니다!</color>");
    }

    public void SetHit(Health health)
    {
        StartCoroutine(SetHitCoroutine(health));
    }

    private IEnumerator SetHitCoroutine(Health health)
    {
        if (!health.gameObject.CompareTag("Player")) yield break;
        while (vignette.intensity.value < 0.5f)
        {
            yield return null;
            vignette.intensity.value += 0.01f;
        }
        vignette.active = true;
        yield return new WaitForSeconds(0.1f);
        while (vignette.intensity.value > 0f)
        {
            yield return null;
            vignette.intensity.value -= 0.01f;
        }
        vignette.active = false;
    }
}
