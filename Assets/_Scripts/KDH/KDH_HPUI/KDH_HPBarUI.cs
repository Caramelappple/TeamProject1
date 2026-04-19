using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KDH_HealthBarUI : MonoBehaviour
{
    public Image hpBarImage;
    public KDH_Health healthResource; //플레이어를 연결

    [Header("체력 닳는 효과들")]
    public float lerpSpeed = 2.0f; //닳는 속도
    public float shakeDuration = 0.2f;
    public float shakeAmount = 5f;

    private float _previousHealth;
    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private Coroutine _shakeCoroutine;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalPosition = _rectTransform.anchoredPosition;

        if (healthResource != null )
        {
            _previousHealth = healthResource.Value;

            if (healthResource.MaxValue > 0)
            {
                hpBarImage.fillAmount = (float)healthResource.Value / healthResource.MaxValue;
            }
        }
    }


    private void Update() //실시간으로 체력바가 바꿔지기 위해 Update를 사용
    {
        //방어 코드
        if (hpBarImage == null || healthResource == null || healthResource.MaxValue <= 0) return;

        //조건들
        if (healthResource.Value < _previousHealth)
        {
            //흔들기 실행
            if (_shakeCoroutine != null) StopCoroutine(_shakeCoroutine);
            _shakeCoroutine = StartCoroutine(ShakeRoutine());
        }

        //현재 체력을 이전 체력으로 덮어쓰기
        _previousHealth = healthResource.Value;

        //부드럽게 스르륵 깎이는 효과
        float targetFill = (float)healthResource.Value / healthResource.MaxValue;
        hpBarImage.fillAmount = Mathf.Lerp(hpBarImage.fillAmount, targetFill, Time.deltaTime * lerpSpeed);
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