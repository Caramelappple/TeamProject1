using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public Vector3 ShakeOffset { get; private set; } // 카메라 이동 스크립트가 읽어감
    private Coroutine shakeCoroutine;

    void Awake() => instance = this;

    public void Shake(float duration, float magnitude)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            ShakeOffset = new Vector3(
                Random.Range(-1f, 1f) * magnitude,
                Random.Range(-1f, 1f) * magnitude,
                0f
            );
            elapsed += Time.deltaTime;
            yield return null;
        }
        ShakeOffset = Vector3.zero;
        shakeCoroutine = null;
    }
}