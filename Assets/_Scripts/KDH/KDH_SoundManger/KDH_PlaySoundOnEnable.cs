using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KDH_PlaySoundOnEnable : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private Image flashImage;
    public float fadeSpeed = 2.0f;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(StartFlash());
        if (audioSource != null)
        {
            audioSource.Play(); // 사운드 재생
        }
    }

    IEnumerator StartFlash()
    {
        // 즉시 화면을 꽉 채우게 1(불투명)로 설정
        Color tempColor = flashImage.color;
        tempColor.a = 1f;
        flashImage.color = tempColor;

        // 서서히 알파값을 0으로 줄임
        while (tempColor.a > 0)
        {
            tempColor.a -= Time.unscaledDeltaTime * fadeSpeed;
            flashImage.color = tempColor;
            yield return null;
        }
        
        tempColor.a = 0f;
        flashImage.color = tempColor;
    }
}
