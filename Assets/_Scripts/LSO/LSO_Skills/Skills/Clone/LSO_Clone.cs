using System.Collections;
using UnityEngine;

public class LSO_Clone : MonoBehaviour
{
    [SerializeField] private GameObject swordAxis;
    [SerializeField] private GameObject sword;

    private float _liveTime = 8f;
    private float _attackCooldown = 0.5f;
    private float _attackRange = 1.5f;
    private int _damage = 10;
    private bool _canAttack = true;

    private void OnEnable()
    {
        sword.SetActive(false);
        Destroy(gameObject, _liveTime);
    }

    private void Update()
    {
        if (!_canAttack) return;

        GameObject nearest = GetNearest();
        if (nearest == null) return;

        StartCoroutine(Attack(nearest));
    }

    private GameObject GetNearest()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _attackRange);

        GameObject result = null;
        float minDist = float.MaxValue;

        foreach (Collider2D target in targets)
        {
            if (!target.CompareTag("Enemy")) continue;

            float dist = Vector2.Distance(transform.position, target.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                result = target.gameObject;
            }
        }
        return result;
    }

    private IEnumerator Attack(GameObject target)
    {
        _canAttack = false;

        if (target != null && target.TryGetComponent<Health>(out Health enemyHealth))
        {
            // 넉백 없이 데미지만
            DamageData data = new DamageData(enemyHealth, _damage);
            enemyHealth.GetDamage(data);

            // 검 방향 설정
            Vector3 dir = (target.transform.position - transform.position).normalized;
            sword.transform.position = transform.position + dir;
            sword.SetActive(true);

            yield return new WaitForSeconds(0.2f);
            sword.SetActive(false);
        }

        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }
}