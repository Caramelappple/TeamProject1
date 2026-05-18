using System.Collections;
using UnityEngine;

public class KDH_CreditsController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 100f;
    [SerializeField] private float disappearYPosition = -1200f; // 사라질 좌표

    [SerializeField] private GameObject parent;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            rectTransform.anchoredPosition += Vector2.down * scrollSpeed * Time.deltaTime;

            if (rectTransform.anchoredPosition.y <= disappearYPosition)
            {
                StartCoroutine(OutEnding());
            }
        }
    }

    private IEnumerator OutEnding()
    {
        scrollSpeed = 0;
        yield return new WaitForSeconds(7f);
        parent.SetActive(false);
    }
}
