using UnityEngine;

public class KDH_BGM : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject; // 배경음악의 주인이 될 녀석
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (_gameObject != null && audioSource != null)
        {
            audioSource.Play();
        }
    }
}
