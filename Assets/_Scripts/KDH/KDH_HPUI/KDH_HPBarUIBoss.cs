using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KDH_HealthBarBossUI : MonoBehaviour
{
    public static KDH_HealthBarBossUI instance;
    private KDH_PlaySoundOnEnable fade;

    [Header("UI 표시 설정")]
    // ★ 중요: 체력바 UI의 비주얼 요소(이미지, 텍스트 등)들을 담고 있는 하위 오브젝트를 연결할 칸입니다.
    public GameObject uiContent;

    public Image hpBarImage;
    public Health healthResource; // 보스 연결

    [Header("체력 닳는 효과들")]
    public float lerpSpeed = 2.0f; // 닳는 속도
    public float shakeDuration = 0.2f;
    public float shakeAmount = 5f;

    public float _previousHealth;
    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private Coroutine _shakeCoroutine;

    private void Awake()
    {
        if (instance == null) instance = this;

        _rectTransform = GetComponent<RectTransform>();
        _originalPosition = _rectTransform.anchoredPosition;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드되면 먼저 보스를 찾습니다.
        FindBoss();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalPosition = _rectTransform.anchoredPosition;
        FindBoss();
    }

    private void Update()
    {
        // ★ 만약 보스가 없다면 실시간으로 계속 찾습니다. (이 스크립트는 이제 꺼지지 않으므로 작동합니다!)
        if (healthResource == null)
        {
            FindBoss();
            return; // 보스를 찾을 때까지 아래 Update 연출 코드는 실행하지 않음
        }

        // 방어 코드
        if (hpBarImage == null || healthResource.MaxValue <= 0) return;

        // 체력 감소 조건 체크 및 흔들기
        if (healthResource.Value < _previousHealth)
        {
            if (_shakeCoroutine != null) StopCoroutine(_shakeCoroutine);
            _shakeCoroutine = StartCoroutine(ShakeRoutine());
        }

        _previousHealth = healthResource.Value;

        // 부드럽게 스르륵 깎이는 효과
        float targetFill = (float)healthResource.Value / healthResource.MaxValue;
        hpBarImage.fillAmount = Mathf.Lerp(hpBarImage.fillAmount, targetFill, Time.deltaTime * lerpSpeed);
    }

    // 보스를 찾고 UI를 켜고 끄는 핵심 함수
    private void FindBoss()
    {
        //GameObject boss = GameObject.FindGameObjectWithTag("Enemy");

        //if (boss != null)
        //{
        //    healthResource = boss.GetComponent<Health>();
        //}
        //else
        //{
        //    healthResource = null;
        //}

        // 보스가 존재하고 Health 컴포넌트가 있다면?
        if (healthResource != null)
        {
            // UI 내용물만 활성화! (스크립트 자체는 꺼지지 않음)
            if (uiContent != null) uiContent.SetActive(true);

            _previousHealth = healthResource.Value;
            if (healthResource.MaxValue > 0)
            {
                hpBarImage.fillAmount = (float)healthResource.Value / healthResource.MaxValue;
            }
        }
        else
        {
            // 보스가 없다면 UI 내용물만 숨기기! (스크립트는 계속 깨어있음)
            if (uiContent != null) uiContent.SetActive(false);
        }
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            _rectTransform.anchoredPosition = new Vector2(_originalPosition.x, _originalPosition.y + y);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _rectTransform.anchoredPosition = _originalPosition;
    }
}