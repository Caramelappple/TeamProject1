using UnityEngine;
using System.Collections;

public class JHY_WarningZone : MonoBehaviour
{
    [SerializeField] private GameObject warningZonePrefab;
    [SerializeField] private float maxRadius = 2.5f;   // 실제 최종 반경
    [SerializeField] private float growTime = 1.5f;
    [SerializeField] private int damage = 30;
    [SerializeField] private LayerMask playerLayer;

    public void Warning()
    {
        Vector3 targetPos = transform.position; // 나중에 착지 위치로 교체 가능
        StartCoroutine(SkillRoutine(targetPos));
    }

    private IEnumerator SkillRoutine(Vector3 centerPos)
    {
        GameObject zone = Instantiate(warningZonePrefab, centerPos, Quaternion.identity);

        float timer = 0f;

        while (timer < growTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / growTime);

            float currentRadius = Mathf.Lerp(0.1f, maxRadius, t);

            // 원형 스프라이트가 "지름 1" 기준이라고 가정
            float diameter = currentRadius * 2f;
            zone.transform.localScale = new Vector3(diameter, diameter, 1f);

            yield return null;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(centerPos, maxRadius, playerLayer);

        foreach (var hit in hits)
        {
            Debug.Log($"맞은 대상: {hit.name}");
            // hit.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }

        Destroy(zone);
    }
}