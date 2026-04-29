using Unity.VisualScripting;
using UnityEngine;

public class KDH_SoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("UI 사운드 클립")]
    public AudioClip hoverSound;
    public AudioClip clicksound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.ignoreListenerPause = true;
    }

    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clicksound);
    }
}
