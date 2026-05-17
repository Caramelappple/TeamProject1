using UnityEngine;

public class KDH_CreditsController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 100f; // 떨어지는 속도
    [SerializeField] private float disappearYPosition = -1200f; // 사라질 기준 Y 좌표

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += Vector2.down * scrollSpeed * Time.deltaTime;

        if (rectTransform.anchoredPosition.y <= disappearYPosition)
        {
            gameObject.SetActive(false);
            // 씬에서 아예 지우고 싶다면: Destroy(gameObject);
        }
    }
}
