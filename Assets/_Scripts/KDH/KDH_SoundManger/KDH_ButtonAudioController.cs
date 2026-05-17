using UnityEngine;

public class KDH_ButtonAudioController : MonoBehaviour
{
    public AudioClip clickSound; // 재생할 소리 파일

    public void OnButtonClick()
    {
        AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);

        gameObject.SetActive(false);
    }
}
