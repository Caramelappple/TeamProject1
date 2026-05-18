using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_PoisonDamage : MonoBehaviour,LSO_ISkill
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float interval = 1f;

    private Dictionary<GameObject, float> timer = new Dictionary<GameObject, float>();

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!timer.ContainsKey(other.gameObject))
        {
            timer[other.gameObject] = 0f;
        }

        timer[other.gameObject] += Time.deltaTime;

        if (timer[other.gameObject] >= interval)
        {
            Health hp = other.GetComponent<Health>();

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

    public void UseSkill(GameObject player)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator CoolTime(float time)
    {
        throw new System.NotImplementedException();
    }
}
