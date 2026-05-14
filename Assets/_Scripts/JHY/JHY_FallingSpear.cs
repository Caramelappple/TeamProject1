using UnityEngine;
using System.Collections;

public class FallingSpear : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float explodeDelay = 1f;

    private Vector2 targetPos;
    private bool hasTarget;
    private bool hasStopped;
    private bool hasExploded;

    public void SetTarget(Vector2 target)
    {
        targetPos = target;
        hasTarget = true;
    }

    private void Update()
    {
        if (!hasTarget || hasStopped || hasExploded) return;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) <= 0.05f)
        {
            transform.position = targetPos;
            hasStopped = true;
            StartCoroutine(ExplodeAfterDelay());
        }
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explodeDelay);

        hasExploded = true;

        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, -225f);
    }
}
