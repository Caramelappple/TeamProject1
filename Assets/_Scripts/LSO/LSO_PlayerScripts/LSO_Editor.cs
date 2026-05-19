using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
        health.OnDamage += (data) => SetLow(health);
     }

    private void SetHit(Health health)
    {
        StartCoroutine(SetHitCoroutine(health));
    }

    private void SetLow(Health health)
    {
        StartCoroutine(LowHealth(health));
    }

    private IEnumerator SetHitCoroutine(Health health)
    {
        vignette.color.value = new Color32(255, 170, 179, 255);
        if (!health.gameObject.CompareTag("Player")) yield break;
        CameraShake.instance.Shake(0.15f, 0.12f);
        vignette.intensity.value = 0.48f;
        vignette.active = true;
        yield return new WaitForSeconds(0.2f);
        while (vignette.intensity.value > 0f)
        {
            yield return null;
            vignette.intensity.value -= 0.007f;
        }
        vignette.active = false;
    }

    private IEnumerator LowHealth(Health health)
    {
        if (!health.gameObject.CompareTag("Player")) yield break;

        if (health.Value / health.MaxValue <= 0.1f)
        {
            vignette.color.value = new Color32(152, 139, 141, 255);
            vignette.intensity.value = 0.48f;
            vignette.active = true;
            yield return new WaitForSeconds(0.2f);
        }
        
        // 체력이 10% 이하인 동안 계속 실행
        while (health.Value / health.MaxValue <= 0.1f)
        {
            yield return new WaitForSeconds(1f);
            CameraShake.instance.Shake(1f, 0.001f);
        }
        
        while (vignette.intensity.value > 0f)
        {
            yield return null;
            vignette.intensity.value -= 0.007f;
        }
        vignette.active = false;
    }
}
