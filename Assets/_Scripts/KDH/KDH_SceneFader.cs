using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KDH_SceneFader : MonoBehaviour
{
    public static KDH_SceneFader Instance { get; private set; }

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private CanvasGroup blockerCanvasGroup;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float darkHoldDuration = 3.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ★ [보정] 게임이 시작되자마자 내 자식 구조(Canvas/FadeCanvas)를 완벽히 찾아 조준합니다.
        FindCanvasComponents();

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
            StartCoroutine(FadeIn());
        }
    }

    private void FindCanvasComponents()
    {
        // 하이어라키 구조가 SceneFaderManager -> Canvas -> FadeCanvas 이므로 경로를 정확히 지정합니다.
        Transform fadeTransform = transform.Find("Canvas/FadeCanvas");
        if (fadeTransform != null)
        {
            fadeCanvasGroup = fadeTransform.GetComponent<CanvasGroup>();
        }

        Transform blockerTransform = transform.Find("Canvas/BlockerCanvas");
        if (blockerTransform != null)
        {
            blockerCanvasGroup = blockerTransform.GetComponent<CanvasGroup>();
        }

        // 혹시라도 위 경로로 못 찾았을 때를 대비한 2차 전체 자식 검색 안전장치
        if (fadeCanvasGroup == null || blockerCanvasGroup == null)
        {
            CanvasGroup[] groups = GetComponentsInChildren<CanvasGroup>(true);
            foreach (var cg in groups)
            {
                if (cg.gameObject.name == "FadeCanvas") fadeCanvasGroup = cg;
                if (cg.gameObject.name == "BlockerCanvas") blockerCanvasGroup = cg;
            }
        }
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        SetBlocker(true);
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }
        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 1f;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone) yield return null;

        yield return new WaitForSeconds(darkHoldDuration);
        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        SetBlocker(true);
        float timer = fadeDuration;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }

        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 0f;
        SetBlocker(false);
    }

    private void SetBlocker(bool isBlocked)
    {
        if (blockerCanvasGroup != null) blockerCanvasGroup.blocksRaycasts = isBlocked;
    }
}