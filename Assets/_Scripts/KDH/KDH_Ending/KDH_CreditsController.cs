using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KDH_CreditsController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 100f;
    [SerializeField] private float disappearYPosition = -1200f;
    [SerializeField] private GameObject parent;
    private RectTransform rectTransform;
    private bool isEndingStarted = false;

    void Start() => rectTransform = GetComponent<RectTransform>();

    void Update()
    {
        if (gameObject.activeSelf)
        {
            rectTransform.anchoredPosition += Vector2.down * scrollSpeed * Time.deltaTime;
            if (rectTransform.anchoredPosition.y <= disappearYPosition && !isEndingStarted)
            {
                isEndingStarted = true;
                StartCoroutine(OutEnding());
            }
        }
    }

    private IEnumerator OutEnding()
    {
        scrollSpeed = 0;
        yield return new WaitForSeconds(7f);

        if (parent != null) parent.SetActive(false);

        if (KDH_SceneFader.Instance != null)
            KDH_SceneFader.Instance.FadeToScene("MainMenu");

        yield return new WaitForSeconds(1.0f);

        // 정적 주소만 비워줍니다.
        LSO_PlayerMovement.instance = null;
        NKY_GameManager.instance = null;

        // ★ Single 모드로 메인 메뉴를 불러오면, 씬에 있던 모든 UI와 카메라는 유니티가 알아서 '소각'합니다.
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}