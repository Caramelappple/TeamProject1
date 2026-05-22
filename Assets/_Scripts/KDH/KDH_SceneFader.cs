using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KDH_SceneFader : MonoBehaviour
{
    public static KDH_SceneFader Instance { get; private set; }

    [SerializeField] private CanvasGroup fadeCanvasGroup;   // Sort Order 0 (배경용)
    [SerializeField] private CanvasGroup blockerCanvasGroup; // Sort Order 2 (클릭 방지용)
    [SerializeField] private float fadeDuration = 1.0f;     // 밝아지는 데 걸리는 시간
    [SerializeField] private float darkHoldDuration = 3.0f; // 어두운 상태를 유지할 시간 (3초)
    private void Start()
    {
        // 게임이 맨 처음 시작될 때도 까만 화면이라면 바로 밝아지도록 실행
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f; // 처음엔 까맣게 시작해서
            StartCoroutine(FadeIn());   // 스르륵 밝아지게 만들기
        }
    }
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

    // 외부에서 호출하는 함수 (예: SceneFader.Instance.FadeToScene("NextScene");)
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        SetBlocker(true); // 클릭 방지 켜기
        float timer = 0f;

        // 1. 현재 씬에서 점점 어두워짐 (Fade Out)
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f; // 완전히 어두워짐

        // 2. 다음 씬을 비동기로 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
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
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = timer / fadeDuration;
            }
            yield return null;
        }

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
        }
        SetBlocker(false);
    }

    private void SetBlocker(bool isBlocked)
    {
        // blockerCanvasGroup이 Null이 아닐 때만 작동하도록 안전장치 추가
        if (blockerCanvasGroup != null)
        {
            blockerCanvasGroup.blocksRaycasts = isBlocked;
        }
    }
}
