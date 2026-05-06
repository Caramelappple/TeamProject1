using System.Collections.Generic;
using UnityEngine;

public class KHG_PoisonDamage : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float interval = 1f;

    private Dictionary<GameObject, float> timer = new Dictionary<GameObject, float>();

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 처음 들어온 대상이면 타이머 초기화
        if (!timer.ContainsKey(other.gameObject))
        {
            timer[other.gameObject] = 0f;
        }

        // 시간 누적
        timer[other.gameObject] += Time.deltaTime;

        // 1초 지났으면 데미지
        if (timer[other.gameObject] >= interval)
        {
            KHG_Health hp = other.GetComponent<KHG_Health>();

            if (hp != null)
            {
                //hp.TakeDamage(damage);
            }

            timer[other.gameObject] = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 나가면 타이머 제거 (중요)
        if (timer.ContainsKey(other.gameObject))
        {
            timer.Remove(other.gameObject);
        }
    }
}
