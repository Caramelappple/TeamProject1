using System.Collections.Generic;
using UnityEngine;

public class NKY_SoundManager : MonoBehaviour
{
    public static NKY_SoundManager Instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] private int initialPoolSize = 10;

    [Header("BGM Source")]
    [SerializeField] private AudioSource bgmSource;

    [Header("Sound Data Registry")]
    [SerializeField] private List<NKY_SoundData> registrySoundData = new List<NKY_SoundData>();

    private List<AudioSource> sfxPool = new List<AudioSource>();
    private Dictionary<string, NKY_SoundData> soundDictionary = new Dictionary<string, NKY_SoundData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManager();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManager()
    {
        // 1. 딕셔너리 세팅
        foreach (var data in registrySoundData)
        {
            if (data != null && !soundDictionary.ContainsKey(data.soundName))
            {
                soundDictionary.Add(data.soundName, data);
            }
        }

        // 2. BGM 소스 세팅
        if (bgmSource == null)
        {
            GameObject bgmObj = new GameObject("BGM_Source");
            bgmObj.transform.SetParent(transform);
            bgmSource = bgmObj.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.spatialBlend = 0f; // 완전한 2D
        }

        // 3. 2D SFX 풀 생성
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewPoolObject();
        }
    }

    private AudioSource CreateNewPoolObject()
    {
        GameObject sfxObj = new GameObject("SFX_Object");
        sfxObj.transform.SetParent(transform);

        AudioSource source = sfxObj.AddComponent<AudioSource>();
        source.spatialBlend = 0f; // 2D 고정
        source.playOnAwake = false;

        sfxObj.SetActive(false);
        sfxPool.Add(source);
        return source;
    }

    private AudioSource GetAvailableSource()
    {
        foreach (var source in sfxPool)
        {
            if (!source.gameObject.activeSelf)
            {
                source.gameObject.SetActive(true);
                return source;
            }
        }

        AudioSource newSource = CreateNewPoolObject();
        newSource.gameObject.SetActive(true);
        return newSource;
    }

    // [배경음 재생]
    public void PlayBGM(string soundName)
    {
        if (!soundDictionary.TryGetValue(soundName, out NKY_SoundData data)) return;
        if (bgmSource.clip == data.clip && bgmSource.isPlaying) return;

        bgmSource.clip = data.clip;
        bgmSource.outputAudioMixerGroup = data.mixerGroup;
        bgmSource.volume = data.volume;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // [효과음 재생] 위치 값 없이 오직 이름으로만 호출!
    public void PlaySFX(string soundName)
    {
        if (!soundDictionary.TryGetValue(soundName, out NKY_SoundData data))
        {
            Debug.LogWarning($"SoundManager2D: {soundName}을 찾을 수 없습니다.");
            return;
        }

        AudioSource source = GetAvailableSource();

        source.clip = data.clip;
        source.outputAudioMixerGroup = data.mixerGroup;
        source.volume = data.volume;
        source.pitch = Random.Range(data.minPitch, data.maxPitch); // 피치 변동으로 자연스럽게

        source.Play();

        StartCoroutine(DisableSourceAfterPlayback(source));
    }

    private System.Collections.IEnumerator DisableSourceAfterPlayback(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length + 0.1f);
        source.Stop();
        source.gameObject.SetActive(false);
    }
}
