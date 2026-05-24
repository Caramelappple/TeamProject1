using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LSO_Editor : MonoBehaviour
{
    //[SerializeField] private AudioClip[] clips;
    [SerializeField] private NKY_SoundData[] soundData; // 나강윤 추가
    
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
        health.OnDamage += (data) => SetHit(health,data);
        //health.OnDamage += (data) => SetLow(health);
     }

    private void SetHit(Health health, DamageResultData data)
    {
        StartCoroutine(SetHitCoroutine(health, data));
    }

    private IEnumerator SetHitCoroutine(Health health , DamageResultData data)
    {
        if (data.giver)
        {
            if (!health.gameObject.CompareTag("Player")) yield break;
        } //LSO_SoundManager.Instance.SfxPlay(clips[Random.Range(0, clips.Length)]);
        NKY_SoundManager.Instance.PlaySFX(soundData[Random.Range(0, soundData.Length)].soundName); // 나강윤 추가
        
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
