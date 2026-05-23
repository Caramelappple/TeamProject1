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

        // 정적 데이터 및 싱글톤 주소 완전 포맷
        PlayerPrefs.DeleteAll();
        LSO_PlayerMovement.instance = null;
        NKY_GameManager.instance = null;

        // 부모 품에 숨어있든 말든 DontDestroyOnLoad에 있는 것 중 페이더 빼고 다 죽입니다.
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.buildIndex == -1)
            {
                if (obj.GetComponent<KDH_SceneFader>() != null || obj.name.ToLower().Contains("fader") || obj.name.Contains("SceneFaderManager"))
                    continue;

                obj.transform.SetParent(null);
                DestroyImmediate(obj);
            }
        }

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}