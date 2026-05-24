using UnityEngine;
using UnityEngine.EventSystems;

public class KDH_UISound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("사운드 설정")]
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && audioSource != null)
        {
            //PlayerOneShot은 사운드가 겹쳐도 소리가 끊기지 않게 하는 역할이다.
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}