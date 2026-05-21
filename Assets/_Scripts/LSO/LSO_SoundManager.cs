using UnityEngine;

public class LSO_SoundManager : MonoBehaviour
{
    public static LSO_SoundManager Instance;

    public AudioSource bgmSource;  // BGM 전용
    public AudioSource sfxSource;  // SFX 전용 (PlayOneShot 사용)

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void SfxPlay(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);  // 동시 재생 OK
    }
}