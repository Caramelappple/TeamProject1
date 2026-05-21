using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LSO_Editor : MonoBehaviour
{
    [SerializeField] private AudioClip[] clip;
    private int _clipIndex;
    
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
        //health.OnDamage += (data) => SetLow(health);
     }

    private void SetHit(Health health)
    {
        StartCoroutine(SetHitCoroutine(health));
    }

    private IEnumerator SetHitCoroutine(Health health)
    {
        
        if (!health.gameObject.CompareTag("Player")) yield break;
        LSO_SoundManager.Instance.SfxPlay("Attack", clip[_clipIndex]);
        _clipIndex = (_clipIndex + 1) % clip.Length;
        
        vignette.color.value = new Color32(255, 170, 179, 255);
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

  
}
