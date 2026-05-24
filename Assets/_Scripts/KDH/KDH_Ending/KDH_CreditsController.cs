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

        // ★ [여기가 핵심] 메인 메뉴로 가기 직전, 무적으로 살아있던 애들을 수동으로 파괴합니다!
        GameObject manager = GameObject.FindWithTag("GameManager"); // 또는 이름으로 찾기
        if (manager != null) Destroy(manager);

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) Destroy(player);

        // UI Canvas도 태그나 이름으로 찾아서 파괴
        GameObject ui = GameObject.Find("UI"); // 에디터상의 실제 UI 최상위 오브젝트 이름
        if (ui != null) Destroy(ui);

        GameObject eventSystem = GameObject.Find("EventSystem");
        if (eventSystem != null) Destroy(eventSystem);

        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null) Destroy(mainCamera);

        // 정적 주소 초기화
        LSO_PlayerMovement.instance = null;
        NKY_GameManager.instance = null;

        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}